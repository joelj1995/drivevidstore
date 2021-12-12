using DriveVidStore_Api.Integrations;
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
        // TODO: Use dependency injection
        private readonly FireBaseIntegration _fb = new FireBaseIntegration();

        public async Task<bool> CreateAccount(CreateAccountRequest createAccountRequest)
        {
            try
            {
                await _fb.CreateAccount(createAccountRequest.Email, createAccountRequest.Password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
