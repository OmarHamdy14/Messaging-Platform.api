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
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            if (string.IsNullOrEmpty(Id.ToString())) return BadRequest(new { Message = "Chat-Id is not found" });
            try
            {
                var chat = await _chatService.GetById(Id);
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
            if(!ModelState.IsValid) return BadRequest(ModelState);           
            try
            {
                var res = await _chatService.Create(model);
                if (res.IsSuccess) return Ok(new { Message = res.Message });
                else return BadRequest(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpPut("Update/{Id}")]
        public async Task<IActionResult> Update(Guid ChaId, [FromBody]UpdateChatDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var res = await _chatService.Update(ChaId, model);
                if (res.IsSuccess) return Ok(new { Message = res.Message });
                else return BadRequest(new { Message = res.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [HttpDelete("Delete/{ChaId}")]
        public async Task<IActionResult> Delete(Guid ChaId)
        {
            if (string.IsNullOrEmpty(ChaId.ToString())) return BadRequest(new {Message = "Chat-Id is not found"});
            try
            {
                var res = await _chatService.Delete(ChaId);
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
