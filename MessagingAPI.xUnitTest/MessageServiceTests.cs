using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessagingAPI.xUnitTest
{
    public class MessageServiceTests
    {
        private readonly MessageService _messageService;
        private readonly Mock<IEntityBaseRepository<Message>> _mockRepo;
        private readonly Mock<IEntityBaseRepository<MessageImage>> _mockMessageImageBase;
        private readonly Mock<IChatService> _mockChatService;
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly Mock<ICloudinaryService> _mockCloudinaryService;
        private readonly Mock<IMapper> _mockMapper;
        public MessageServiceTests()
        {
            _mockRepo = new Mock<IEntityBaseRepository<Message>>();
            _mockMessageImageBase = new Mock<IEntityBaseRepository<MessageImage>>();
            _mockChatService = new Mock<IChatService>();
            _mockAccountService = new Mock<IAccountService>();
            _mockCloudinaryService = new Mock<ICloudinaryService>();
            _mockMapper = new Mock<IMapper>();


            _messageService = new MessageService(_mockRepo.Object, _mockMapper.Object, _mockChatService.Object, _mockAccountService.Object, _mockCloudinaryService.Object, _mockMessageImageBase.Object);
        }
        [Fact]
        public async Task GetById_ShouldReturnMessage_WhenExists()  
        {
            var msg = new Message() { Id = Guid.NewGuid(), Content = "hiii" };

            _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Message,bool>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(msg);

            var result = await _messageService.GetById(msg.Id);

            Assert.NotNull(result); 
            Assert.Equal(msg.Content.Length, result.Content.Length);
            Assert.Equal(msg.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAfterDatetimeWithChatId_ShouldReturnListOfUsers_IfExists()
        {
            var ChatId = Guid.NewGuid();
            var msgs = new List<Message>()
            {
                new Message() { Id = Guid.NewGuid(), Content="1111111" , ChatId=ChatId},
                new Message() { Id = Guid.NewGuid(), Content="2222222" , ChatId=ChatId},
                new Message() { Id = Guid.NewGuid(), Content="3333333" , ChatId=ChatId}
            };

            _mockRepo.Setup(r => r.GetAll(It.IsAny<Expression<Func<Message, bool>>>(), It.IsAny<string>())).ReturnsAsync(msgs);

            var result = await _messageService.GetAllAfterDatetimeWithChatId(DateTime.UtcNow.AddMinutes(-20), ChatId);

            Assert.NotNull(result);
            Assert.Equal(result.Count, 3);


            
        }


        [Fact]
        public async Task Create_ShouldCreateMessage_AndReturnSuccess()
        {
            var dto = new CreateMessageDTO();
            var msg = new Message();

            _mockMapper.Setup(m => m.Map<Message>(dto)).Returns(msg);

            _mockRepo.Setup(r => r.Create(It.IsAny<Message>())).Returns(Task.CompletedTask);

            var result = await _messageService.Create(dto);
        }
    }
}
