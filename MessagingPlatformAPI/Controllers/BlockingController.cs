using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockingController : ControllerBase
    {
        private readonly IBlockingService _blockingService;
        public BlockingController(IBlockingService blockingService)
        {
            _blockingService = blockingService;
        }
        [HttpGet("GetByRecordId/{recId}")]
        public async Task<IActionResult> GetByRecordId(Guid recId)
        {
            if (string.IsNullOrEmpty(recId.ToString())) return BadRequest();
            try{
                var res = await _blockingService.GetByRecordId(recId);
                if(res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpGet("GetByBlockerIdAndBlodkedId/{blockedId}")]
        public async Task<IActionResult> GetByBlockerIdAndBlodkedId(string blockedId) 
            // is passing users'IDs in request body safe or not ??
        {
            if (string.IsNullOrEmpty(blockedId)) return BadRequest();
            try
            {
                var blockerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = await _blockingService.GetByBlockerIdAndBlodkedId(blockerId,blockedId);
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpGet("GetAllByBlockerId")]
        public async Task<IActionResult> GetAllByBlockerId()
        {
            try
            {
                // difference between ClaimTypes & ClaimsPrincipal & ClaimsIdentity & Claim ?????
                var blockerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = await _blockingService.GetAllByBlockerId(blockerId);
                if (!res.Any()) return NotFound(); 
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPost("Cerate/{blockedId}")]
        public async Task<IActionResult> Cerate(string blockedId)
        {
            if (string.IsNullOrEmpty(blockedId)) return BadRequest();
            try
            {
                var blockerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var res = await _blockingService.Create(blockerId, blockedId);
                if (!res.IsSuccess) return BadRequest(new {Message = "Semething wrong happened"});
                return Ok(new {Message = res.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete/{recId}")]
        public async Task<IActionResult> Delete(Guid recId)
        {
            if (string.IsNullOrEmpty(recId.ToString())) return BadRequest();
            try
            {
                var res = await _blockingService.Delete(recId);
                if (!res.IsSuccess) return BadRequest(new { Message = "Something went wrong." }); 
                // is return badrequest here right, or there is another right reesponse ???
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
