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
        public async Task<UserSettings> GetByUserId(string UserId)
        {
            return await _base.Get(s => s.UserId == UserId);
        }
        public async Task<SimpleResponseDTO<UserSettings>> Create(string UserId)
        {
            var userSettings = new UserSettings() { UserId = UserId };
            await _base.Create(userSettings);
            return new SimpleResponseDTO<UserSettings>() { IsSuccess = true, Object=userSettings };
        } 
        public async Task<SimpleResponseDTO<UserSettings>> Update(Guid SettingsId, UpdateUserSettingsDTO model)
        {
            var settings = await _base.Get(s => s.Id == SettingsId);
            _mapper.Map(settings, model);
            await _base.Update(settings);
            return new SimpleResponseDTO<UserSettings>() { IsSuccess=true, Object=settings };
        }
    }
}
