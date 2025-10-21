using MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IReactionService
    {
        Task<Reaction> GetById(Guid Id);
        Task<List<Reaction>> GetAllByMessageId(Guid MessageId);
        Task<SimpleResponseDTO> Create(CreateReactionDTO model);
        Task<SimpleResponseDTO> Update(Guid ChaId, UpdateReactionDTO model);
        Task<SimpleResponseDTO> Delete(Guid ReactionId);
    }
}
