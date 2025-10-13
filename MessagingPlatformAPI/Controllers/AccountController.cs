using MessagingPlatformAPI.Helpers.DTOs.AccountDTOs;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("FindById/{userId}")]
        public async Task<IActionResult> FindById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("This {UserId} is not right", userId);
                return BadRequest();
            }
            try
            {
                var user = await _accountService.FindById(userId);
                if (user is null)
                {
                    _logger.LogWarning("This {UserId} is not found", userId);
                    return NotFound();
                }
                _logger.LogInformation("Retreiving {fname} {lname} is done", user.FirstName, user.LastName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpGet("FindByUserName/{name}")]
        public async Task<IActionResult> FindByUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("This {name} is not right", name);
                return BadRequest();
            }
            try
            {
                var user = await _accountService.FindByUserName(name);
                if (user is null)
                {
                    _logger.LogWarning("This {name} is not found", name);
                    return NotFound();
                }
                _logger.LogInformation("Retreiving {fname} {lname} is done", user.FirstName, user.LastName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _accountService.GetAllUsers();
                if (!users.Any())
                {
                    _logger.LogInformation("No users are found");
                    return NotFound();
                }
                _logger.LogInformation("Retreving users is done");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterationDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var authModel = await _accountService.Register(model);
                if (!authModel.IsAuthenticated)
                {
                    _logger.LogWarning("Registration of this user with '{email}' is not succedded", model.Email);
                    return BadRequest(authModel);
                }
                _logger.LogInformation("{FName} {lName} registered successfully", model.FirstName, model.LastName);
                return Ok(authModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO model)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("User's inputs are wrong");
                return BadRequest(ModelState);
            }
            try
            {
                var authModel = await _accountService.GetTokenAsync(model);
                if (!authModel.IsAuthenticated)
                {
                    _logger.LogWarning("Registration of this user with '{email}' is not succedded", model.Email);
                    return BadRequest(authModel);
                }
                var user = await _accountService.FindByEmail(model.Email);
                _logger.LogInformation("{FName} {lName} login successfully", user.FirstName, user.LastName);
                return Ok(authModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpPut("Update/{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserDTO model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("This {UserId} is not right", userId);
                return BadRequest();
            }
            try
            {
                var user = await _accountService.FindById(userId);
                if (user is null)
                {
                    _logger.LogWarning("This {UserId} is not found", userId);
                    return NotFound();
                }

                var result = await _accountService.Update(user, model);
                if (result.Succeeded)
                {
                    _logger.LogInformation("{FName} {lName} is updated successfully", user.FirstName, user.LastName);
                    return Ok(new { Message = "Update is succeeded." });
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
        [Authorize]
        [HttpPut("ChangePassword/{userId}")]
        public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordDTO model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("This {UserId} is not right", userId);
                return BadRequest();
            }
            try
            {
                var user = await _accountService.FindById(userId);
                if (user is null)
                {
                    _logger.LogWarning("This {UserId} is not found", userId);
                    return NotFound();
                }

                var result = await _accountService.ChangePassword(user, model);
                if (result)
                {
                    _logger.LogInformation("{FName} {lName}'password is updated successfully", user.FirstName, user.LastName);
                    return Ok(new { Message = "Changing Password is done successfully." });
                }
                return Ok(new { Message = "Changing Password is not done." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Something went wrong." });
            }
        }
    }
}
