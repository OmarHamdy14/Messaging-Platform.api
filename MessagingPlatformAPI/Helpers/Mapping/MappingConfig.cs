using AutoMapper;
using MessagingPlatformAPI.Helpers.DTOs.AccountDTOs;
using MessagingPlatformAPI.Helpers.DTOs.GroupChatDTO;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Helpers.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, RegisterationDTO>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDTO>().ReverseMap();

            CreateMap<GroupChat, CerateGroupChatDTO>().ReverseMap();
            CreateMap<GroupChat, UpdateGroupChatDTO>().ReverseMap();

            CreateMap<Message, CreateMessageDTO>().ReverseMap();
            CreateMap<Message, UpdateMessageDTO>().ReverseMap();
        }
    }
}
