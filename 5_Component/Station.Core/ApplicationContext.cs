using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Station.Core.AppSettings;
using Station.Core.Http;
using Station.Core.Model;

namespace Station.Core
{
    public class ApplicationContext:IApplicationContext
    {
        public ApplicationContext(IOptions<Settings> settingsOptions)
        {
            Settings = settingsOptions.Value;
        }

        /// <summary>
        /// 配置文件信息
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// 请求基本信息
        /// </summary>
        public HttpRequestLog CurrentLogInfo { get; set; }

        /// <summary>
        /// 当前请求用户信息
        /// </summary>
        public HttpRequestUserInfo CurrentLoginUserInfo { get; set; }

        /// <summary>
        /// token信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 设置基本信息
        /// </summary>
        /// <param name="myToken"></param>
        /// <returns></returns>
        public async Task SetCurrentLogInfo(string myToken)
        {
            this.Token = myToken;
            string userLoginStr = SessionHelper.GetSession(this.Token);
            CurrentLoginUserInfo = JsonConvert.DeserializeObject<HttpRequestUserInfo>(userLoginStr);
            var context = AppHttpContext.Current;
            CurrentLogInfo = new HttpRequestLog
            {
                ActionUrl = AppHttpContext.Current.Request.Path,
                //UserId = UserId,
                User = CurrentLoginUserInfo,
                Headers = context.Request.Headers,
                Host = context.Request.Host,
                Query = context.Request.Query,
                Form = context.Request.HasFormContentType ? context.Request.Form : null,
                Body = await ReadRequestBody(context.Request, Encoding.UTF8),
                TraceIdentifier = AppHttpContext.Current.TraceIdentifier,
                Protocol = AppHttpContext.Current.Request.Protocol,
                IpAddress = AppHttpContext.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? AppHttpContext.Current.Connection.RemoteIpAddress.ToString(),
                Scheme = context.Request.Scheme,
                StartDate = DateTime.Now,
                ContentType = context.Request.ContentType,
                Method = context.Request.Method,
                Result = new ResponseResultModel()
            };
        }

        /// <summary>
        /// 读取请求正文
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private async Task<string> ReadRequestBody(HttpRequest request, Encoding encoding)
        {
            var body = "";
            request.EnableBuffering();
            if (request.ContentLength == null ||
                !(request.ContentLength > 0) ||
                !request.Body.CanSeek)
            {
                return body;
            }
            request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
            {
                body = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;
            return body;
        }
    }
}