using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;
        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            if (string.IsNullOrEmpty(Id.ToString()))
            {
                _logger.LogWarning ("This id '{Id}' is wrong", Id);
                return BadRequest(new { Message = "Chat-Id is not found" });
            }
            try
            {
                var chat = await _chatService.GetById(Id);
                _logger.LogInformation("Retreiving chat with {chatId}", Id);
                return Ok(chat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]CerateChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var res = await _chatService.Create(model);
                if (res.IsSuccess)
                {
                    _logger.LogInformation("Create Chat {name} is succedded", model.Name);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Create Chat {name} is not succedded; {problem}", model.Name, res.Message);
                    return BadRequest(new { Message = res.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> Update(Guid ChaId, [FromBody]UpdateChatDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var res = await _chatService.Update(ChaId, model);
                if (res.IsSuccess)
                {
                    _logger.LogInformation("Uppdate Chat with Id '{Id}' is succedded", ChaId);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Update Chat with Id '{id}' is not succedded; {problem}", ChaId, res.Message);
                    return BadRequest(new { Message = res.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete/{ChaId}")]
        public async Task<IActionResult> Delete(Guid ChaId)
        {
            if (string.IsNullOrEmpty(ChaId.ToString()))
            {
                _logger.LogWarning("This Id '{Id}' is wrong", ChaId);
                return BadRequest(new { Message = "Chat-Id is not found" });
            }
            try
            {
                var res = await _chatService.Delete(ChaId);
                if (res.IsSuccess)
                {
                    _logger.LogInformation("Delete Chat with Id '{Id}' is succedded", ChaId);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Delete Chat with Id '{Id}' is not succedded; {problem}", ChaId, res.Message);
                    return BadRequest(new { Message = res.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
