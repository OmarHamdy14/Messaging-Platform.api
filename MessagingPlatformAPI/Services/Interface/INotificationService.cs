using MessagingPlatformAPI.Helpers.DTOs.NotificationDTOs;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface INotificationService
    {
        Task PushNotification(string UserId, CreatePushNotificationDTO model);
    }
}
