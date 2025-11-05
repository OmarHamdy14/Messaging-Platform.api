using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IEntityBaseRepository<UserSettings> _base;
        private readonly IMapper _mapper;
        public UserSettingsService(IEntityBaseRepository<UserSettings> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<UserSettings> GetById(string UserId)
        {
            return await _base.Get(s => s.UserId == UserId);
        }
        public async Task<SimpleResponseDTO> Create(string UserId)
        {
            await _base.Create(new UserSettings() { UserId = UserId });
            return new SimpleResponseDTO() { IsSuccess = true };
        } 
        public async Task<SimpleResponseDTO> Update(Guid SettingsId, UpdateUserSettingsDTO model)
        {
            var settings = await _base.Get(s => s.Id == SettingsId);
            _mapper.Map(settings, model);
            await _base.Update(settings);
            return new SimpleResponseDTO() { IsSuccess=true };
        }
    }
}
