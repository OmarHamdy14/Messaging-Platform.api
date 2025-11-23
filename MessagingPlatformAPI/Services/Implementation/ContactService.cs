using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IEntityBaseRepository<Contact> _base;
        public ContactService(IEntityBaseRepository<Contact> @base)
        {
            _base = @base;
        }
        public async Task<Contact> Get(string userId, string contactId)
        {
            return await _base.Get(c => c.UserId == userId && c.ContactId == contactId);
        }
        public async Task<bool> IsContact(string userId, string contactId)
        {
            return await _base.Get(c => c.UserId == userId && c.ContactId == contactId) == null ? false : true;
        }
        public async Task<SimpleResponseDTO<Contact>> Create(string userId, string contactId)
        {
            if(await IsContact(userId,contactId)) return new SimpleResponseDTO<Contact>() { IsSuccess = false, Message = "there is already contact" }
            var conct = new Contact() { ContactId = contactId, UserId = userId };
            await _base.Create(conct);
            return new SimpleResponseDTO<Contact>() { IsSuccess = true, Message = "", Object = conct };
        }
        public async Task<SimpleResponseDTO<Contact>> Delete(string userId, string contactId)
        {
            var conct = await Get(userId, contactId);
            await _base.Remove(conct);
            return new SimpleResponseDTO<Contact>() { IsSuccess = true, Message = "", Object = conct }; // is passing contact wrong?
        }
    }
}
