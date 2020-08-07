using System.Threading.Tasks;
using Station.Core.AppSettings;
using Station.Core.Model;

namespace Station.Core
{
    public interface IApplicationContext
    {
        /// <summary>
        /// 当前请求信息
        /// </summary>
        HttpRequestLog CurrentLogInfo { get; set; }

        /// <summary>
        /// 当前请求用户信息
        /// </summary>
        public HttpRequestUserInfo CurrentLoginUserInfo { get; set; }

        /// <summary>
        /// 授权字符串
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 设置当前用户登录信息
        /// </summary>
        Task SetCurrentLogInfo(string myToken);

        /// <summary>
        /// 设置
        /// </summary>
        Settings Settings { get; set; }
    }
}