using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using MessagingPlatformAPI.SignalrConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IChatHub _chatHub;
        private readonly IAccountService _accountService;
        public MessageController(IMessageService messageService, IHubContext<ChatHub> hub, IChatHub chatHub, IAccountService accountService)
        {
            _messageService = messageService;
            _hub = hub;
            _chatHub = chatHub;
            _accountService = accountService;
        }
        [HttpGet("GetAllByChatId/{ChatId}")]
        public async Task<IActionResult> GetAllByChatId(Guid ChatId)
        {
            if (string.IsNullOrEmpty(ChatId.ToString())) return BadRequest();
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var res = await _messageService.Create(model);
                if (res.IsSuccess)
                {
                    // await _chatHub.SendMessage(await _accountService.FindById(model.UserId), model.Content, model.ChatId);
                    return Ok(new { Message = res.Message });
                }
                else return BadRequest(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{MessageId}")]
        public async Task<IActionResult> Update(Guid MessageId, [FromBody] UpdateMessageDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(MessageId.ToString())) return BadRequest();
            try
            {
                var res = await _messageService.Update(MessageId, model);
                if (res.IsSuccess) return Ok(new { Message = res.Message });
                else return BadRequest(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete/{MessageId}")]
        public async Task<IActionResult> Delete(Guid MessageId)
        {
            if (string.IsNullOrEmpty(MessageId.ToString())) return BadRequest();
            try
            {
                var res = await _messageService.Delete(MessageId);
                if (res.IsSuccess) return Ok(new { Message = res.Message });
                else return BadRequest(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
