using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class MessageStatusService : IMessageStatusService
    {
        private readonly IEntityBaseRepository<MessageStatus> _base;
        private readonly IMapper _mapper;
        public MessageStatusService(IEntityBaseRepository<MessageStatus> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<List<MessageStatus>> GetAllByMessageId(Guid MessageId)
        {
            return await _base.GetAll(c => c.MessageId == MessageId);
        }
    }
}
