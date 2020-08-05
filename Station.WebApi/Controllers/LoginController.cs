﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceReference;
using Station.Core.Login;
using Station.Model.UserDto;
using Station.Repository.Token;
using Station.Repository.User;

namespace Station.WebApi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController:ControllerBase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        public LoginController(ITokenRepository tokenRepository,IUserRepository userRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
           
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserDtoParameter user)
        {
            UserInfo userInfo = await _userRepository.Login(user.UserName, user.Password);
            UserDto returnUserDto = new UserDto();
            
            if (userInfo?.UserName != null && userInfo.Status == UserInfo.StatusType.正常)
            {
                string myToken = await _tokenRepository.GetToken(userInfo.UserName, DateTime.Now, DateTime.Now.AddDays(1));

                HttpContext.Session.SetString(myToken,JsonSerializer.Serialize(new HttpRequestLogUserModel(userInfo)));
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