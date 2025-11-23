using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.Enums;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IEntityBaseRepository<Contact> _base;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IPresenseTrackerService _presenseTrackerService;
        public ContactService(IEntityBaseRepository<Contact> @base, IUserSettingsService userSettingsService,IPresenseTrackerService presenseTrackerService)
        {
            _base = @base;
            _userSettingsService = userSettingsService;
            _presenseTrackerService = presenseTrackerService;
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
        public async Task<DateTime> GetLastSeen(string requesterId, string targetId)
        {
            var setng = await _userSettingsService.GetByUserId(targetId);

            return setng.Privacy switch
            {
                LastSeenPrivacy.Nobody => null,
                LastSeenPrivacy.Everyone => await _presenseTrackerService.GetLastSeen(targetId),
                LastSeenPrivacy.ContactsOnly => await IsContact(requesterId, targetId) ?
                                                await _presenseTrackerService.GetLastSeen(targetId) : null,
                _ => null
            };
        }
    }
}
