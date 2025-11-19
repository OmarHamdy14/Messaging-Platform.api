using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IEntityBaseRepository<UserConnection> _base;
        private readonly IMapper _mapper;
        public UserConnectionService(IEntityBaseRepository<UserConnection> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<UserConnection> GetByConnectionId(string ConnectionId)
        {
            return await _base.Get(c => c.ConnectionId == ConnectionId);
        }
        public async Task<List<UserConnection>> GetAllByUserId(string UserId)
        {
            return await _base.GetAll(u => u.UserId == UserId);
        }
        public async Task<SimpleResponseDTO<UserConnection>> Create(CreateUserConnectionDTO model)
        {
            var userConn = _mapper.Map<UserConnection>(model);
            await _base.Create(userConn);
            return new SimpleResponseDTO<UserConnection>() { IsSuccess = true, Message = "user Connection creation is done", Object=userConn };
        }
        public async Task<SimpleResponseDTO<UserConnection>> Delete(string connectionId)
        {
            var userConn = await _base.Get(c => c.ConnectionId == connectionId);
            if (userConn == null) return new SimpleResponseDTO<UserConnection>() { IsSuccess = false, Message = "Chat is not found" };
            await _base.Remove(userConn);
            return new SimpleResponseDTO<UserConnection>() { IsSuccess = true, Message = "user Connection deletion is done", Object=userConn };
        }
    }
}
