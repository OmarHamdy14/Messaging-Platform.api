namespace MessagingPlatformAPI.Services.Interface
{
    public interface IPresenseTrackerService
    {
        Task UserConnectedAsync(string userId, string connectionId);
        Task UserDisconnectedAsync(string userId, string connectionId);
        Task<bool> IsUserOnline(string userId);
        Task<List<string>> GetOnlineUsers();
        Task<List<string>> GetUserConnections(string userId);
    }
}
