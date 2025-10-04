using MessagingPlatformAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.SignalrConfig
{
    public interface IChatHub
    {
        Task SendMessage(ApplicationUser user, string msg, Guid GroupId);
        Task AddUserToGroup(Guid GroupId, HubCallerContext context);
    }
}
