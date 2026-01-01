using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.JWTconfig;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingAPI.xUnitTest
{
    public class AccountServiceTests
    {
        private readonly AccountService _accountService;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IEntityBaseRepository<ProfileImage>> _profileImageBase;
        private readonly Mock<ICloudinaryService> _cloudinaryService;
        private readonly Mock<IUserSettingsService> _userSettingsService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<JWT> _jwt;
        private readonly Mock<IConfiguration> _confg;
        public AccountServiceTests()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>();
            _profileImageBase = new Mock<IEntityBaseRepository<ProfileImage>>();
            _cloudinaryService = new Mock<ICloudinaryService>();
            _userSettingsService = new Mock<IUserSettingsService>();
            _mapper = new Mock<IMapper>();
            _jwt = new Mock<JWT>();
            _confg = new Mock<IConfiguration>();

            _accountService = new AccountService(_userManager.Object, _mapper.Object, _jwt.Object, _cloudinaryService.Object, _profileImageBase.Object, _userSettingsService.Object, _confg.Object);
        }
        [Fact]
        public async Task FindById_ShouldReturnUser()
        {
            var Id = "111";
            _userManager.Setup(r => r.FindByIdAsync(Id)).ReturnsAsync(new ApplicationUser { Id = "111" });

            var result = await _accountService.FindById(Id);
            Assert.NotNull(result);
            Assert.Equal(result.Id, Id);
        }

        [Fact]
        public async Task FindByName_ShouldReturnUser()
        {
            var name = "omar";
            _userManager.Setup(r => r.FindByNameAsync(name)).ReturnsAsync(new ApplicationUser { Id = "111" });

            var result = await _accountService.FindByUserName(name);
            Assert.NotNull(result);
            Assert.Equal(result.UserName, name);
        }

        [Fact]
        public async Task FindByEmail_ShouldReturnUser()
        {
            var email = "hhh@gg";
            _userManager.Setup(r => r.FindByEmailAsync(email)).ReturnsAsync(new ApplicationUser { Id = "111" });

            var result = await _accountService.FindByEmail(email);
            Assert.NotNull(result);
            Assert.Equal(result.Email, email);
        }


    }
}
