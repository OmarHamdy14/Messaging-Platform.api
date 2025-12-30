using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MessagingAPI.xUnitTest
{
    public class ChatServiceTests
    {
        private readonly ChatService _chatService;
        private readonly Mock<IEntityBaseRepository<Chat>> _chatMockRepo;
        private readonly Mock<IEntityBaseRepository<ChatImage>> _chatImageMockRepo;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICloudinaryService> _cloudinaryMock;

        public ChatServiceTests()
        {
            _chatMockRepo = new Mock<IEntityBaseRepository<Chat>>();
            _mapperMock = new Mock<IMapper>();
            _chatImageMockRepo = new Mock<IEntityBaseRepository<ChatImage>>();
            _cloudinaryMock = new Mock<ICloudinaryService>();
            _chatService = new ChatService(_chatMockRepo.Object, _mapperMock.Object, _chatImageMockRepo.Object, _cloudinaryMock.Object);
        }
        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var expectedRes = new Chat { Id = Guid.NewGuid() , Name="Test"  };
            _chatMockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<Chat, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                         .ReturnsAsync(expectedRes);


            var result = await _chatService.GetById(expectedRes.Id);

            Assert.NotNull(result);
            Assert.Equal(expectedRes.Name, result.Name);

            _chatMockRepo.Verify(r => r.Get(
                It.IsAny<Expression<Func<Chat, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
            ), Times.Once);
        }
    }
}