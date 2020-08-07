using System.Threading.Tasks;
using ServiceReference;

namespace Station.WcfServiceProxy.ServiceWrapper.User
{
    public interface IUserServiceWrapper
    {
        Task<UserInfo> Login(string userName, string pwd);
    }
}