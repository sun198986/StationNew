using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using IBM.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Station.Aspect.Authorization;
using Station.Aspect.Exception;
using Station.Aspect.Filter;
using Station.Core;
using Station.Core.AppSettings;
using Station.EFCore.IbmDb;
using Station.Core.ETag;
using Station.Core.Http;
using Station.Core.Swagger;
using Station.SortApply.Helper;

namespace Station.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory ConsoleLoggerFactory =
           LoggerFactory.Create(builder =>
           {
               builder.AddFilter((category, level) =>
                   category == DbLoggerCategory.Database.Command.Name
                   && level == LogLevel.Information).AddConsole();
           });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InitSwaggerConfig();
            //Etag ����
            services.InitEtagConfig();

            services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("120sCacheProfile",new CacheProfile
                {
                    Duration = 120
                });
                options.Filters.Add(typeof(CustomerExceptionFilterAttribute));//ȫ���쳣����
                options.Filters.Add(typeof(CustomerResultFilterAttribute));
                options.Filters.Add(typeof(CustomerAuthorizeFilterAttribute));
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .ConfigureApiBehaviorOptions(setup =>//������Ϣ������� FluentValidation
            {
                setup.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "",
                        Title = "�д���",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "�뿴��ϸ��Ϣ",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            })
            .AddXmlDataContractSerializerFormatters();//֧��xml����

            services.AddDbContext<IbmDbContext>(options =>
            {
                options.UseDb2(Configuration.GetConnectionString("DevelopDB"), action =>
                {

                }).UseLoggerFactory(ConsoleLoggerFactory);//��ӡsql�ű�
            });

            services.Scan(scan => scan.FromAssemblies(Assembly.Load("Station.Repository"), Assembly.Load("Station.WcfServiceProxy"), Assembly.Load("Station.SortApply.Helper"))
               .AddClasses().UsingAttributes());//����ע��
            services.AddScoped<IApplicationContext, ApplicationContext>();
            services.AddScoped<PropertyMappingCollection>();

            services.AddAutoMapper(config =>
            {
                config.ForAllMaps((a, b) => b.ForAllMembers(opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)));
            }, Assembly.Load("Station.Entity"), Assembly.Load("Station.Model"),Assembly.Load("Station.WcfServiceProxy"));

            services.Configure<MvcOptions>(config =>
            {
                var newtonSoftJsonOutputFormatter =
                    config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();
                newtonSoftJsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.company.hateoas+json");
            });
            services.AddDistributedMemoryCache();
            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromDays(1);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UserSwaggerConfig();

            AppHttpContext.Configure(app.ApplicationServices.
                GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());

            app.UseHttpCacheHeaders();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
