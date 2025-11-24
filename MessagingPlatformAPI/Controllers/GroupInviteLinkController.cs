using MessagingPlatformAPI.Helpers.DTOs.GroupInviteLinkDTOs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupInviteLinkController : ControllerBase
    {
        private readonly IGroupInviteLinkService _groupInviteLinkService;
        public GroupInviteLinkController(IGroupInviteLinkService groupInviteLinkService)
        {
            _groupInviteLinkService = groupInviteLinkService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLinkDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var res = await _groupInviteLinkService.CreateLink(model);
            if(res.IsSuccess) return Ok(res);
            return BadRequest(res);
        }
        [HttpGet]
        public async Task<IActionResult> JoinViaLink([FromBody] JoinViaLinkDTO model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var res = await _groupInviteLinkService.JoinGroup(model);
            if (res) return Ok();
            return BadRequest();
        }
    }
}
