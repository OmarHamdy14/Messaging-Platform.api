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
        public ChatHub(IAccountService accountService, IChatMembersService chatMembersService)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
        }
        public async Task SendMessage(ApplicationUser user, string msg, Guid GroupId)
        {
            await Clients.Group(GroupId.ToString()).ReceiveMessage(user.UserName,msg);
        }
        public async Task AddUserToGroup(Guid GroupId)
        {
            var user = await _accountService.FindById(Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString());
        }
        public override async Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier;
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
