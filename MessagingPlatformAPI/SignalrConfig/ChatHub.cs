using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.SignalrConfig
{
    public class ChatHub : Hub<IChatMethod>
    {
        private readonly IAccountService _accountService;
        public async Task SendMessage(ApplicationUser user, string msg, Guid GroupId)
        {
            await Clients.Group(GroupId.ToString()).ReceiveMessage(user.UserName,msg);
        }
        public async Task AddUserToGroup(Guid GroupId, HubCallerContext context)
        {
            var user = await _accountService.FindById(context.UserIdentifier);

            await Groups.AddToGroupAsync(context.ConnectionId, GroupId.ToString());
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
