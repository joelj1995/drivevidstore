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

        [Route("create-account")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest loginRequest)
        {
            bool createAccountSucceeded = await _authService.CreateAccount(loginRequest);
            if (createAccountSucceeded)
            {
                return Ok(new CreateAccountResponse { Succeeded = true });
            }
            else
            {
                return StatusCode(500, new CreateAccountResponse { Succeeded = false, Error = "A user with that email already exists." });
            }
        }
    }
}
