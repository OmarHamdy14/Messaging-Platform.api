using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IContactService
    {
        Task<Contact> Get(string userId, string contactId);
        Task<bool> IsContact(string userId, string contactId);
        Task<SimpleResponseDTO<Contact>> Create(string userId, string contactId);
        Task<SimpleResponseDTO<Contact>> Delete(string userId, string contactId);

        Task<string> GetLastSeen(string requesterId, string targetId);
    }
}
