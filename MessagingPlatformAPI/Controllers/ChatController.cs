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
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpGet]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var chat = await _chatService.GetById(Id);
            return Ok(chat);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CerateChatDTO model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var res = await _chatService.Create(model);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
        [HttpPut]
        public async Task<IActionResult> Update(Guid ChaId, UpdateChatDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var res = await _chatService.Update(ChaId, model);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid ChaId)
        {
            var res = await _chatService.Delete(ChaId);
            if (res.IsSuccess) return Ok(new { Message = res.Message });
            else return BadRequest(new { Message = res.Message });
        }
    }
}
