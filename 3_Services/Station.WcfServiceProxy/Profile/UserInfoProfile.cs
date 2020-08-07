using ServiceReference;
using Station.Core.Model;

namespace Station.WcfServiceProxy.Profile
{
    public class UserInfoProfile:AutoMapper.Profile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfo, HttpRequestUserInfo>();
        }
    }
}