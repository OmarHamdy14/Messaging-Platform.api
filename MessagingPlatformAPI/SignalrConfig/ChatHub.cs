using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.SignalrConfig
{
    public class ChatHub : Hub<IChatMethod>, IChatHub
    {
        private readonly IAccountService _accountService;
        private readonly IChatMembersService _chatMembersService;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(IAccountService accountService, IChatMembersService chatMembersService, ILogger<ChatHub> logger)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
            _logger = logger;
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
            var groups = await _chatMembersService.GetAllByUserId(UserId);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
            }
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
