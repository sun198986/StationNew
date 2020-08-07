using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceReference;
using Station.Core.Model;
using Station.Model.UserDto;
using Station.WcfServiceProxy.ServiceWrapper.Token;
using Station.WcfServiceProxy.ServiceWrapper.User;

namespace Station.WebApi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController:ControllerBase
    {
        private readonly ITokenServiceWrapper _tokenServiceWrapper;
        private readonly IUserServiceWrapper _userServiceWrapper;
        private readonly IMapper _mapper;

        public LoginController(ITokenServiceWrapper tokenServiceWrapper,IUserServiceWrapper userServiceWrapper,IMapper mapper)
        {
            _tokenServiceWrapper = tokenServiceWrapper;
            _userServiceWrapper = userServiceWrapper;
            _mapper = mapper;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserDtoParameter user)
        {
            UserInfo userInfo = await _userServiceWrapper.Login(user.UserName, user.Password);
            UserDto returnUserDto = new UserDto();
            
            if (userInfo?.UserName != null && userInfo.Status == UserInfo.StatusType.正常)
            {
                string myToken = await _tokenServiceWrapper.InsertToTokenAsync(userInfo.UserName, DateTime.Now, DateTime.Now.AddDays(1),"","","");

                HttpRequestUserInfo requestUserInfo = _mapper.Map<HttpRequestUserInfo>(userInfo);

                HttpContext.Session.SetString(myToken, JsonSerializer.Serialize(requestUserInfo));
                returnUserDto.MyToken = myToken;
                returnUserDto.LoginResult = true;
            }
            else
            {
                returnUserDto.LoginResult = false;
            }

            return Ok(returnUserDto);
        }
    }
}