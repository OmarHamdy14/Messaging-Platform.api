using MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly ILogger _logger;
        public ReactionController(IReactionService reactionService, ILogger logger)
        {
            _reactionService = reactionService;
            _logger = logger;
        }
        [HttpGet("GetAllByMessageId/{MessageId}")]
        public async Task<IActionResult> GetAllByMessageId(Guid MessageId)
        {
            if (string.IsNullOrEmpty(MessageId.ToString())) return BadRequest();
            try
            {
                var reacts = await _reactionService.GetAllByMessageId(MessageId);
                _logger.LogInformation("Retreiving reacts of message with {Id}", MessageId);
                if (!reacts.Any()) return NotFound();
                return Ok(reacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateReactionDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var res = await _reactionService.Create(model);
                if (!res.IsSuccess) return BadRequest(new { Message=res.Message });
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{ReactionId}")]
        public async Task<IActionResult> Update(Guid ReactionId, [FromBody] UpdateReactionDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(ReactionId.ToString())) return BadRequest();
            try
            {
                var res = await _reactionService.Update(ReactionId,model);
                if (!res.IsSuccess) return BadRequest(new { Message = res.Message });
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid ReactionId)
        {
            if (string.IsNullOrEmpty(ReactionId.ToString())) return BadRequest();
            try
            {
                var res = await _reactionService.Delete(ReactionId);
                if (!res.IsSuccess) return BadRequest(new { Message = res.Message });
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
