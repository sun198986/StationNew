using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using ServiceReference;
using Station.Core;

namespace Station.WcfServiceProxy.ServiceWrapper.Token
{
    [ServiceDescriptor(typeof(ITokenServiceWrapper), ServiceLifetime.Transient)]
    public class TokenServiceWrapper:ITokenServiceWrapper
    {
        private readonly TokenClient _tokenClient;

        public TokenServiceWrapper(IApplicationContext applicationContext)
        {
            _tokenClient = WcfFactory.GetWcfClient<TokenClient>(applicationContext.Settings.WcfUrl);
        }
        public async Task ValidateGuidAsync(string token)
        {
            await _tokenClient.ValidateGuidAsync(token);
        }

        public async Task<string> InsertToTokenAsync(string userName, DateTime startDateTime, DateTime endDateTime, string ipAddress, string machine,
            string macAddress)
        {
            return await _tokenClient.InsertToTokenAsync(userName, startDateTime, endDateTime, ipAddress, machine,
                macAddress);
        }
    }
}