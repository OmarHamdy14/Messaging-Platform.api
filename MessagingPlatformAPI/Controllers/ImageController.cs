using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        public ImageController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(string ImagePublicId)
        {
            if (string.IsNullOrEmpty(ImagePublicId))
            { 
                return BadRequest();
            }
            try
            {
                var res = await _cloudinaryService.DeleteFile(ImagePublicId);
                if (!res.IsSuccess) return BadRequest(new { Message = res.Message });
                return Ok(res.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
