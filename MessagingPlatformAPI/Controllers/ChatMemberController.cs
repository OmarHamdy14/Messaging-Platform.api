using MessagingPlatformAPI.Helpers.DTOs.ChatMemberDTOs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMemberController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IChatMembersService _chatMembersService;
        private readonly ILogger<ChatMemberController> _logger;
        public ChatMemberController(IAccountService accountService, ILogger<ChatMemberController> logger, IChatMembersService chatMembersService)
        {
            _accountService = accountService;
            _logger = logger;
            _chatMembersService = chatMembersService;
        }
        [Authorize]
        [HttpGet("GetAllMembersOfChatByChatId/{ChatId}")]
        public async Task<IActionResult> GetAllMembersOfChatByChatId(Guid ChatId)
        {
            if (string.IsNullOrEmpty(ChatId.ToString())) return BadRequest();
            try
            {
                var res = await _chatMembersService.GetAllByChatId(ChatId);
                _logger.LogInformation("Retreving memebers is done");
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpGet("GetAllChatsOfUserByUserId/{UserId}")]
        public async Task<IActionResult> GetAllChatsOfUserByUserId(string UserId)
        {
            if (string.IsNullOrEmpty(UserId)) return BadRequest();
            try
            {
                var res = await _chatMembersService.GetAllByUserId(UserId);
                _logger.LogInformation("Retreving memebers is done");
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpPut("MakeAdmin")]
        public async Task<IActionResult> MakeAdmin([FromBody] RecordDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var res = await _chatMembersService.MakeAdmin(model);
                if (!res.IsSuccess) return NotFound(new { Message = res.Message });
                _logger.LogInformation("Retreving users is done");
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] RecordDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var res = await _chatMembersService.Delete(model);
                if (!res.IsSuccess) return NotFound(new { Message = res.Message });
                _logger.LogInformation("Deleting users is done");
                return Ok(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
