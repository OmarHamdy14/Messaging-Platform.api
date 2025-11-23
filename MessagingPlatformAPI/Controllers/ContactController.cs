using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService, IUserSettingsService userSettingsService)
        {
            _contactService = contactService;
        }
        public async Task<IActionResult> Get(string userId, string contactId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(contactId)) return BadRequest();
            return Ok(await _contactService.Get(userId, contactId));
        }
        public async Task<IActionResult> Create(string userId, string contactId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(contactId)) return BadRequest();
            var res = await _contactService.Create(userId, contactId);
            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res);
        }
        public async Task<IActionResult> Delete(string userId, string contactId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(contactId)) return BadRequest();
            var res = await _contactService.Delete(userId, contactId);
            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res);
        }
    }
}
