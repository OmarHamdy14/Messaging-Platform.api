using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllByChatId(Guid ChatId)
        {
            var chats = await _messageService.GetAllByChatId(ChatId);
            return Ok(chats);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var res = await _messageService.Create(model);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
        [HttpPut]
        public async Task<IActionResult> Update(Guid MessageId, UpdateMessageDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var res = await _messageService.Update(MessageId, model);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid MessageId)
        {
            var res = await _messageService.Delete(MessageId);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
    }
}
