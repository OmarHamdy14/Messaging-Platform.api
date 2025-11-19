using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IMessageService
    {
        Task<List<Message>> GetAllAfterDatetimeWithChatId(DateTime dt, Guid ChatId);
        Task<List<Message>> GetAllAfterDatetime(DateTime dt);
        Task<Message> GetById(Guid MessageId);
        Task<List<Message>> GetAllByChatId(Guid ChatId);
        /*Task<List<Message>> GetAllByPrivateChatId(Guid PrivateChatId);
        Task<List<Message>> GetAllByGroupChatId(Guid GroupChatId);*/
        Task<SimpleResponseDTO<Message>> Create(CreateMessageDTO model, List<IFormFile> files);
        Task<SimpleResponseDTO<Message>> Update(Guid MessageId, UpdateMessageDTO model);
        Task<SimpleResponseDTO<Message>> Delete(Guid MessageId);
        Task<SimpleResponseDTO<MessageImage>> DeleteMessagePic(string ImagePublicId);
    }
}
