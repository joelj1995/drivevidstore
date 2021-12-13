using DriveVidStore_Api.Integrations;
using DriveVidStore_Api.Models.Profile;
using DriveVidStore_Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly FireBaseIntegration _fb = new FireBaseIntegration();

        public async Task<bool> AddDriveApiKey(string user, AddKeyRequest addKeyRequest)
        {
            return await _fb.AddApiKey(user, addKeyRequest.Name, addKeyRequest.Key);
        }
    }
}
