using MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IUserSettingsService _userSettingsService;
        public SettingsController(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = await _userSettingsService.GetByUserId(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{stgId}")] // is the name of parameter here should be the same as it is in method parameters ???
        public async Task<IActionResult> Update(Guid stgId, [FromBody] UpdateUserSettingsDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var res = await _userSettingsService.Update(stgId, model);
                if(res.IsSuccess) return Ok(res);
                return StatusCode(500, new { Message = "Something went wrong." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
