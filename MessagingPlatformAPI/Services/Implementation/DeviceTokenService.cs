using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.DeviceTokenDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class DeviceTokenService : IDeviceTokenService
    {
        private readonly IEntityBaseRepository<DeviceToken> _base;
        private readonly IMapper _mapper;
        public DeviceTokenService(IEntityBaseRepository<DeviceToken> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<SimpleResponseDTO<DeviceToken>> CreateDeviceToken(string UserId, CreateTokenDTO model)
        {
            var token = _mapper.Map<DeviceToken>(model);
            token.UserId = UserId;
            token.CeratedAt = DateTime.UtcNow;
            await _base.Create(token);
            return new SimpleResponseDTO<DeviceToken>() { IsSuccess=true, Object=token };
        }
        public async Task DeleteAsync(DeviceToken token)
        {
            await _base.Remove(token);
        }
    }
}
