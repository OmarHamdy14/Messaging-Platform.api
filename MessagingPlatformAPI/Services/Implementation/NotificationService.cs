using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MessagingPlatformAPI.Services.Interface;
using FirebaseAdmin.Messaging;
using MessagingPlatformAPI.Helpers.DTOs.NotificationDTOs;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Models;
using Message = FirebaseAdmin.Messaging.Message;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IEntityBaseRepository<DeviceToken> _baseDeviceToken;
        private readonly IDeviceTokenService _deviceTokenService;
        public NotificationService(IEntityBaseRepository<DeviceToken> @base, IDeviceTokenService deviceTokenService)
        {
            _baseDeviceToken = @base;
            _deviceTokenService = deviceTokenService;
        }

        public async Task<bool> PushNotification(string UserId, CreatePushNotificationDTO model)
        {
            var UserDeviceTokens = await _baseDeviceToken.GetAll(d => d.UserId == UserId);
            foreach(var tkn in UserDeviceTokens)
            {
                var message = new Message
                {
                    Token = tkn.Token,
                    Notification = new Notification
                    {
                        Title = model.Title,
                        Body = model.Content
                    }
                };

                try
                {
                    // Returns a message ID string if successful
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    return true;
                    // Console.WriteLine($"Message accepted by FCM. MessageId: {response}");
                    // message is "Delivered"
                }
                catch (FirebaseMessagingException ex)
                {
                    Console.WriteLine($"FCM rejected the message for token {tkn.Token}. Error: {ex.Message}");

                    // Example: remove invalid tokens
                    if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                        ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                    {
                        await _deviceTokenService.DeleteAsync(tkn);
                        return false;
                        //Console.WriteLine($"Removed invalid token: {tkn.Token}");
                    }

                    // Log or handle other errors here
                }
            }
        }
    }
}
