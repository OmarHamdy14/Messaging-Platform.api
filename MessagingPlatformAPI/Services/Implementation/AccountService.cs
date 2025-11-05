using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.AccountDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.JWTconfig;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityBaseRepository<ProfileImage> _profileImageBase;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AccountService(UserManager<ApplicationUser> userManager, IMapper mapper, JWT jwt, ICloudinaryService cloudinaryService, IEntityBaseRepository<ProfileImage> profileImageBase, IUserSettingsService userSettingsService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt;
            _cloudinaryService = cloudinaryService;
            _profileImageBase = profileImageBase;
            _userSettingsService = userSettingsService;
        }
        public async Task<ApplicationUser> FindById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<ApplicationUser> FindByUserName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
        public async Task<AuthModel> Register(RegisterationDTO model, IFormFile profilePic)
        {
            if (await FindByEmail(model.Email) is not null) return new AuthModel() { Message = "Email is already registered." };
            //if (await FindByUserName(model.UserName) is not null) return new AuthModel() { Message = "User Name is already registered." };
            var user = _mapper.Map<ApplicationUser>(model);
            var res = await _userManager.CreateAsync(user);
            if (res.Succeeded)
            {
                var usr = await FindByEmail(model.Email);
                await _userSettingsService.Create(usr.Id);
            }
            var cloudinaryRes = await _cloudinaryService.UploadFile(profilePic);
            if (cloudinaryRes.IsSuccess)
            {
                await _profileImageBase.Create(new ProfileImage() { PublicId = cloudinaryRes.PublicId, Url = cloudinaryRes.Url, UserId = user.Id });
                return new AuthModel() { Message = "Registration is Succeeded.", IsAuthenticated = true };
            }
            return new AuthModel() { Message = res.Errors.ToString() + "/n" + cloudinaryRes.Message };
        }
        public async Task<AuthModel> GetTokenAsync(LogInDTO model)
        {
            //var user = await FindByUserName(model.UserName);
            var user = await FindByEmail(model.Email);
            if (user == null) return new AuthModel() { Message = "Invalid User Name." };
            var PassCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!PassCheck) return new AuthModel() { Message = "Invalid Password." };

            var token = await CreateJwtToken(user);
            return new AuthModel() { Token = new JwtSecurityTokenHandler().WriteToken(token), IsAuthenticated = true, Message = "LogIn is Succeeded." };
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleCalims = new List<Claim>();

            foreach (var role in roles) roleCalims.Add(new Claim("roles", role));

            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("User_ID",user.Id)
            }.Union(userClaims).Union(roleCalims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var JwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.UtcNow.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials,
                claims: claims
            );
            return JwtSecurityToken;
        }
        public async Task<IdentityResult> Update(ApplicationUser user, UpdateUserDTO model)
        {
            _mapper.Map(user, model);
            var res = await _userManager.UpdateAsync(user);
            return res;
        }
        public async Task<bool> ChangePassword(ApplicationUser user, ChangePasswordDTO model)
        {
            if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword)) return false;
            var res = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (res.Succeeded) return true;
            return false;

        }
        public async Task SaveChangesAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task<SimpleResponseDTO> ChangeProfilePic(ApplicationUser User, IFormFile pic)
        {
            var currentPic = await _profileImageBase.Get(p => p.UserId == User.Id);
            if (currentPic != null)
            {
                await _cloudinaryService.DeleteFile(currentPic.PublicId);
                await _profileImageBase.Remove(currentPic);
            }
            var cloudinaryRes = await _cloudinaryService.UploadFile(pic);
            if (cloudinaryRes.IsSuccess)
            {
                await _profileImageBase.Create(new ProfileImage() { PublicId = cloudinaryRes.PublicId, Url = cloudinaryRes.Url, UserId = User.Id });
                return new SimpleResponseDTO() { IsSuccess = true, Message = "Changing profile pidture is done" };
            }
            return new SimpleResponseDTO() { IsSuccess = false };
        }
        public async Task<SimpleResponseDTO> DeleteProfilePic(string ImagePublicId)
        {
            var currentPic = await _profileImageBase.Get(p => p.PublicId == ImagePublicId);
            if (currentPic != null)
            {
                await _cloudinaryService.DeleteFile(ImagePublicId);
                await _profileImageBase.Remove(currentPic);
                return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is done" };
            }
            return new SimpleResponseDTO() { IsSuccess = false, Message = "Deletion is failed" };
        }


    }
}
