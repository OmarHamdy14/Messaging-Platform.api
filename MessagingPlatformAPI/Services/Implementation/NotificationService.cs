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
        public NotificationService(IEntityBaseRepository<DeviceToken> @base)
        {
            _baseDeviceToken = @base;
        }

        public async Task PushNotification(string UserId, CreatePushNotificationDTO model)
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

                await FirebaseMessaging.DefaultInstance.SendAsync(message);     
            }
        }
    }
}
