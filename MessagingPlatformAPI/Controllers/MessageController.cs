using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using MessagingPlatformAPI.SignalrConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IBlockingService _blockingService;
        private readonly IChatMembersService _chatMembersService;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IChatHub _chatHub;
        private readonly IAccountService _accountService;
        private readonly ILogger<MessageController> _logger;
        public MessageController(IMessageService messageService, IHubContext<ChatHub> hub, IChatHub chatHub, IAccountService accountService, ILogger<MessageController> logger, IChatService chatService, IBlockingService blockingService, IChatMembersService chatMembersService)
        {
            _messageService = messageService;
            _hub = hub;
            _chatHub = chatHub;
            _accountService = accountService;
            _logger = logger;
            _chatService = chatService;
            _blockingService = blockingService;
            _chatMembersService = chatMembersService;
        }
        [HttpGet("GetAllByChatId/{ChatId}")]
        public async Task<IActionResult> GetAllByChatId(Guid ChatId)
        {
            if (string.IsNullOrEmpty(ChatId.ToString()))
            {
                _logger.LogWarning("This id '{Id}' is wrong", ChatId);
                return BadRequest(new { Message = "Chat-Id is not found" });
            }
            try
            {
                var chats = await _messageService.GetAllByChatId(ChatId);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateMessageDTO model) // send message __
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            var chat = await _chatService.GetById(model.ChatId);
            if(chat.chatType == Helpers.Enums.ChatType.prv)
            {
                var SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var mems = await _chatMembersService.GetAllByChatId(model.ChatId);
                var RecivId = mems.Where(m => m.MemberId != SenderId).Select(m => m.MemberId).FirstOrDefault();
                
                var IsBlocked = await _blockingService.GetByBlockerIdAndBlodkedId(RecivId, SenderId);
                if (IsBlocked is not null) return BadRequest(new { Message = "You can't send a message" });
            }
            try
            {
                var res = await _messageService.Create(model);
                if (res.IsSuccess)
                {
                    // await _chatHub.SendMessage(await _accountService.FindById(model.UserId), model.Content, model.ChatId);
                    _logger.LogInformation("Create message in chat (with id '{ChatId}') and by user (with id '{UserId}') is succedded", model.ChatId, model.UserId);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Create message in chat (with id '{ChatId}') and by user (with id '{UserId}') is not succedded; {problem}", model.ChatId, model.UserId, res.Message);
                    return BadRequest(new { Message = res.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{MessageId}")]
        public async Task<IActionResult> Update(Guid MessageId, [FromBody] UpdateMessageDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(MessageId.ToString()))
            {
                _logger.LogWarning("This id '{Id}' is wrong", MessageId);
                return BadRequest(new { Message = "Chat-Id is not found" });
            }
            try
            {
                var res = await _messageService.Update(MessageId, model);
                if (res.IsSuccess)
                {
                    _logger.LogInformation("Update message with Id '{Id}' is succedded", MessageId);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Update message with Id '{Id}' is not succedded; {problem}", MessageId, res.Message);
                    return BadRequest(new { Message = res.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete/{MessageId}")]
        public async Task<IActionResult> Delete(Guid MessageId)
        {
            if (string.IsNullOrEmpty(MessageId.ToString()))
            {
                _logger.LogWarning("This id '{Id}' is wrong", MessageId);
                return BadRequest(new { Message = "Chat-Id is not found" });
            }
            try
            {
                var res = await _messageService.Delete(MessageId);
                if (res.IsSuccess)
                {
                    _logger.LogInformation("Delete message with Id '{Id}' is succedded", MessageId);
                    return Ok(new { Message = res.Message });
                }
                else
                {
                    _logger.LogInformation("Delete message with Id '{Id}' is not succedded; {problem}", MessageId, res.Message);
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
