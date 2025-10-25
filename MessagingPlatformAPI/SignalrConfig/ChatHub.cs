using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.SignalrConfig
{
    public class ChatHub : Hub<IChatMethod>, IChatHub
    {
        private readonly IAccountService _accountService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IChatMembersService _chatMembersService;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(IAccountService accountService, IChatMembersService chatMembersService, ILogger<ChatHub> logger, IUserConnectionService userConnectionService)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
            _logger = logger;
            _userConnectionService = userConnectionService;
        }
        public async Task SendMessage(ApplicationUser user, string msg, Guid GroupId)
        {
            await Clients.Group(GroupId.ToString()).ReceiveMessage(user.UserName,msg);
            _logger.LogInformation("Sending message from user '{fname} {lname}' is succedded", user.FirstName, user.LastName);
        }
        public async Task AddUserToGroup(Guid GroupId)
        {
            var user = await _accountService.FindById(Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString());
        }
        public override async Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            user.IsOnline = true;
            await _accountService.SaveChangesAsync(user);

            await _userConnectionService.Create(new CreateUserConnectionDTO() { UserId = UserId ,ConnectionId = Context.ConnectionId });

            var groups = await _chatMembersService.GetAllByUserId(UserId);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.ChatId.ToString());
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userConnectionService.Delete(Context.ConnectionId);

            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            if (!user.UserConnections.Any())  // if the user dont open the app in another device
            {
                user.IsOnline = false;
                user.LastSeen = DateTime.UtcNow;
                await _accountService.SaveChangesAsync(user);
            }
            // if user open the app in another device, he is still online
            await base.OnDisconnectedAsync(exception);
        }
    }
}
