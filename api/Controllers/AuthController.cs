using DriveVidStore_Api.Models.Auth;
using DriveVidStore_Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public LoginResponse Login(string userName, string password)
        {
            return _authService.Login(userName, password);
        }
    }
}
