using MessagingPlatformAPI.Helpers.DTOs.NotificationDTOs;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface INotificationService
    {
        Task<bool> PushNotification(string UserId, CreatePushNotificationDTO model);
    }
}
