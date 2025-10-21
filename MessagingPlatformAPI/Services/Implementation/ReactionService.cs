
using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ReactionService : IReactionService
    {
        private readonly IEntityBaseRepository<Reaction> _base;
        private readonly IMapper _mapper;
        public ReactionService(IEntityBaseRepository<Reaction> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<Reaction> GetById(Guid Id)
        {
            return await _base.Get(c => c.Id == Id);
        }
        public async Task<List<Reaction>> GetAllByMessageId(Guid MessageId)
        {
            return await _base.GetAll(c => c.MessageId == MessageId);
        }
        public async Task<SimpleResponseDTO> Create(CreateReactionDTO model)
        {
            var reac = _mapper.Map<Reaction>(model);
            await _base.Create(reac);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Reaction creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid ChaId, UpdateReactionDTO model)
        {
            var reac = await _base.Get(c => c.Id == ChaId);
            if (reac == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "reaction is not found" };
            _mapper.Map(reac, model);
            await _base.Update(reac);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Reaction update is done" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid ReactionId)
        {
            var reac = await _base.Get(c => c.Id == ReactionId);
            if (reac == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Reaction is not found" };
            await _base.Remove(reac);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Reaction deletion is done" };
        }
    }
}
