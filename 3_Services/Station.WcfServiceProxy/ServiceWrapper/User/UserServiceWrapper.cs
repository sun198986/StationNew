using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using ServiceReference;
using Station.Core;
using Station.WcfServiceProxy.Proxy;

namespace Station.WcfServiceProxy.ServiceWrapper.User
{
    [ServiceDescriptor(typeof(IUserServiceWrapper), ServiceLifetime.Transient)]
    public class UserServiceWrapper:IUserServiceWrapper
    {
        private readonly UserClient _userClient;


        public UserServiceWrapper(IApplicationContext applicationContext)
        {
            _userClient = WcfFactory.GetWcfClient<UserClient>(applicationContext.Settings.WcfUrl); ;
        }

        public async Task<UserInfo> Login(string userName, string pwd)
        {
            return await _userClient.LoginAsync(pwd, userName);
        }
    }
}