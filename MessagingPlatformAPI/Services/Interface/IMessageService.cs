using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IMessageService
    {
        Task<Message> GetById(Guid MessageId);
        Task<List<Message>> GetAllByChatId(Guid ChatId);
        /*Task<List<Message>> GetAllByPrivateChatId(Guid PrivateChatId);
        Task<List<Message>> GetAllByGroupChatId(Guid GroupChatId);*/
        Task<SimpleResponseDTO> Create(CreateMessageDTO model);
        Task<SimpleResponseDTO> Update(Guid MessageId, UpdateMessageDTO model);
        Task<SimpleResponseDTO> Delete(Guid MessageId);
    }
}
