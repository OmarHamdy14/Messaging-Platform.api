using MessagingPlatformAPI.Services.Interface;
using System.Collections.Concurrent;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class PresenseTrackerService : IPresenseTrackerService
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> OnlineUsers = new();

        private static readonly object _lock = new();

        public Task UserConnectedAsync(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (OnlineUsers.ContainsKey(userId))
                    OnlineUsers[userId].Add(connectionId);
                else
                    OnlineUsers[userId] = new HashSet<string> { connectionId };
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnectedAsync(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (!OnlineUsers.ContainsKey(userId))
                    return Task.CompletedTask;

                OnlineUsers[userId].Remove(connectionId);
                if (OnlineUsers[userId].Count == 0)
                    OnlineUsers.Remove(userId, out _);
            }

            return Task.CompletedTask;
        }

        public Task<bool> IsUserOnline(string userId)
        {
            bool isOnline = OnlineUsers.ContainsKey(userId);
            return Task.FromResult(isOnline);
        }

        public Task<List<string>> GetOnlineUsers()
        {
            var users = OnlineUsers.Keys.ToList();
            return Task.FromResult(users);
        }

        public Task<List<string>> GetUserConnections(string userId)
        {
            if (OnlineUsers.TryGetValue(userId, out var connections))
                return Task.FromResult(connections.ToList());

            return Task.FromResult(new List<string>());
        }
    }
}
