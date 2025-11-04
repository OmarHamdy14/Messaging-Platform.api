using MessagingPlatformAPI.Helpers.DTOs.DeviceTokenDTOs;
using MessagingPlatformAPI.Helpers.DTOs.NotificationDTOs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IDeviceTokenService _deviceTokenService;
        public NotificationController(INotificationService notificationService, IDeviceTokenService deviceTokenService)
        {
            _notificationService = notificationService;
            _deviceTokenService = deviceTokenService;
        }
        [HttpPost("CreateDeviceToken")]
        public async Task<IActionResult> CreateDeviceToken([FromBody] CreateTokenDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _deviceTokenService.CreateDeviceToken(UserId, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> PushNotification([FromBody] CreatePushNotificationDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _notificationService.PushNotification(UserId, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
