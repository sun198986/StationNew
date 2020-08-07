using System;
using System.Threading.Tasks;

namespace Station.WcfServiceProxy.ServiceWrapper.Token
{
    public interface ITokenServiceWrapper
    {
        Task ValidateGuidAsync(string token);

        Task<string> InsertToTokenAsync(string userName, DateTime startDateTime, DateTime endDateTime, string ipAddress,
            string machine, string macAddress);
    }
}