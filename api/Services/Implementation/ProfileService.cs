using DriveVidStore_Api.Integrations;
using DriveVidStore_Api.Services.Interface;
using DrivVidStore_Common.Integrations;
using DrivVidStore_Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly FirebaseIntegration _fb = new FirebaseIntegration();

        public async Task AddDriveApiKey(string user, DriveKey addKeyRequest)
        {
            await _fb.AddApiKey(user, addKeyRequest.Name, addKeyRequest.Key);
        }

        public async Task RemoveDriveApiKey(string user, DriveKey addKeyRequest)
        {
            await _fb.RemoveApiKey(user, addKeyRequest.Name, addKeyRequest.Key);
        }

        public async Task<IEnumerable<DriveKey>> GetKeys(string user)
        {
            return await _fb.GetKeys(user);
        }
    }
}
