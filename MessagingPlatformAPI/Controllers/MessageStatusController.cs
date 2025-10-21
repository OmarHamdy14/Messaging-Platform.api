using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageStatusController : ControllerBase
    {
        private readonly IMessageStatusService _messageStatusService;
        private readonly ILogger _logger;
        public MessageStatusController(IMessageStatusService messageStatusService, ILogger logger)
        {
            _messageStatusService = messageStatusService;
            _logger = logger;
        }
        [HttpGet("GetAllByMessageId/{MessageId}")]
        public async Task<IActionResult> GetAllByMessageId(Guid MessageId)
        {
            if(string.IsNullOrEmpty(MessageId.ToString())) return BadRequest();
            try
            {
                var statuses = await _messageStatusService.GetAllByMessageId(MessageId);
                _logger.LogInformation("Retreiving Statuses of message with {Id}", MessageId);
                if(!statuses.Any()) return NotFound();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
