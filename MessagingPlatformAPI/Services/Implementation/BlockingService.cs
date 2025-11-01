using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class BlockingService : IBlockingService
    {
        private readonly IEntityBaseRepository<BlockedUser> _base;
        private readonly ILogger _logger;
        public BlockingService(IEntityBaseRepository<BlockedUser> @base, ILogger logger)
        {
            _base = @base;
            _logger = logger;
        }
        public async Task<bool> IsBlocked(string blockerId, string blockedId)
        {
            var rec = await _base.Get(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
            return rec is not null ? true : false;
        }
        public async Task<BlockedUser> GetByRecordId(Guid recId)
        {
            return await _base.Get(b => b.Id == recId);
        }
        public async Task<BlockedUser> GetByBlockerIdAndBlodkedId(string blockerId, string blockedId)
        {
            return await _base.Get(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
        }
        public async Task<List<BlockedUser>> GetAllByBlockerId(string blockerId)
        {
            return await _base.GetAll(b => b.BlockerId == blockerId);
        }
        public async Task<SimpleResponseDTO> Create(string blockerId, string blockedId)
        {
            // do i have ti use try&catch here, ot using it in controller is enough ???
            await _base.Create(new BlockedUser() { BlockerId = blockerId, BlockedId = blockedId, CreatedAt = DateTime.UtcNow });
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Creation id done" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid recId)
        {
            var rec = await GetByRecordId(recId);
            await _base.Remove(rec);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion id done" };
        }
    }
}
