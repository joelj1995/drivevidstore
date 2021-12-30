using DriveVidStore_Api.Models.Auth;
using DriveVidStore_Api.Services.Interface;
using DrivVidStore_Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Route("api-keys")]
        [HttpPost]
        public async Task<IActionResult> AddKey(DriveKey addKeyRequest)
        {
            var userId = User.Claims.First(c => c.Type == "user_id").Value;
            await _profileService.AddDriveApiKey(userId, addKeyRequest);
            return Ok();
        }

        [Route("api-keys")]
        [HttpDelete]
        public async Task<IActionResult> DeleteKey(DriveKey addKeyRequest)
        {
            var userId = User.Claims.First(c => c.Type == "user_id").Value;
            await _profileService.RemoveDriveApiKey(userId, addKeyRequest);
            return Ok();
        }

        [Route("api-keys")]
        [HttpGet]
        public async Task<IActionResult> GetKeys()
        {
            var userId = User.Claims.First(c => c.Type == "user_id").Value;
            var keys = await _profileService.GetKeys(userId);
            return Ok(keys);
        }
    }
}
