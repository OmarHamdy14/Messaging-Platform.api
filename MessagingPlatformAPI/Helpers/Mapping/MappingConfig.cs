using AutoMapper;
using MessagingPlatformAPI.Helpers.DTOs.AccountDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.DeviceTokenDTOs;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs;
using MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Models;
namespace MessagingPlatformAPI.Helpers.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, RegisterationDTO>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDTO>().ReverseMap();

            CreateMap<Message, CreateMessageDTO>().ReverseMap();
            CreateMap<Message, UpdateMessageDTO>().ReverseMap();

            CreateMap<Chat, CerateChatDTO>().ReverseMap();
            CreateMap<Chat, UpdateChatDTO>().ReverseMap();

            CreateMap<DeviceToken, CreateTokenDTO>().ReverseMap();

            CreateMap<Reaction, CreateReactionDTO>().ReverseMap();
            CreateMap<Reaction, UpdateReactionDTO>().ReverseMap();

            CreateMap<UserSettings, UpdateUserSettingsDTO>().ReverseMap();

            CreateMap<UserConnection, CreateUserConnectionDTO>().ReverseMap();
        }
    }
}