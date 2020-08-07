using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Station.Core;
using Station.WcfServiceProxy.ServiceWrapper.Token;

namespace Station.Aspect.Authorization
{
    public class CustomerAuthorizeFilterAttribute: Attribute,IAsyncAuthorizationFilter, IFilterMetadata
    {
        private readonly IApplicationContext _applicationContext;
        private readonly ITokenServiceWrapper _tokenServiceWrapper;

        public CustomerAuthorizeFilterAttribute(IApplicationContext applicationContext, ITokenServiceWrapper tokenServiceWrapper)
        {
            _applicationContext = applicationContext;
            _tokenServiceWrapper = tokenServiceWrapper;
            ;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor) context.ActionDescriptor)
                .MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                string myToken = context.HttpContext.Request.Headers["token"];
                if (!string.IsNullOrEmpty(myToken))
                {
                    //验证令牌
                    try
                    {
                        await _tokenServiceWrapper.ValidateGuidAsync(myToken);
                        await _applicationContext.SetCurrentLogInfo(myToken);
                    }
                    catch (System.Exception e)
                    {
                        context.Result = new UnauthorizedObjectResult(e.Message);
                    }
                }
            }
        }
    }
}