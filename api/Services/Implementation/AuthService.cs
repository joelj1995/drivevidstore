using DriveVidStore_Api.Models.Auth;
using DriveVidStore_Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Implementation
{
    public class AuthService : IAuthService
    {
        public LoginResponse Login(string userName, string password)
        {
            return new LoginResponse()
            {
                Token = "1234"
            };
        }
    }
}
