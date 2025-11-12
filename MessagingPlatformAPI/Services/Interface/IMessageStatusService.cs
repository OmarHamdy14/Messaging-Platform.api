using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IMessageStatusService
    {
        Task<List<MessageStatus>> GetAllByMessageId(Guid MessageId);
        Task CreateStatus(MessageStatus msgSt);
    }
}
