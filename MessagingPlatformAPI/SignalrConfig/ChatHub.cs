using FirebaseAdmin.Messaging;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.NotificationDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ReactionDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Helpers.Enums;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MessagingPlatformAPI.SignalrConfig
{
    public class ChatHub : Hub//<IChatMethod>
    {
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IBlockingService _blockingService;
        private readonly IChatMembersService _chatMembersService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IMessageStatusService _messageStatusService;
        private readonly IPresenseTrackerService _presenseTrackerService;
        private readonly INotificationService _notificationService;
        private readonly IReactionService _reactionService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub
            (IAccountService accountService, IChatMembersService chatMembersService, ILogger<ChatHub> logger, IUserConnectionService userConnectionService, 
            IMessageService messageService, IChatService chatService, IBlockingService blockingService, IMessageStatusService messageStatusService, 
            IPresenseTrackerService presenseTrackerService, INotificationService notificationService, IReactionService reactionService,
            IUserSettingsService userSettingsService)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
            _logger = logger;
            _userConnectionService = userConnectionService;
            _messageService = messageService;
            _chatService = chatService;
            _blockingService = blockingService;
            _messageStatusService = messageStatusService;
            _presenseTrackerService = presenseTrackerService;
            _notificationService = notificationService;
            _reactionService = reactionService;
            _userSettingsService = userSettingsService;
        }
        public async Task SendMessage(CreateMessageDTO model, List<IFormFile> files)
        {
            var chat = await _chatService.GetById(model.ChatId);

            /*
             * *************************************************
            if (chat.chatType == Helpers.Enums.ChatType.prv) // if i block someone named Ada, ada still can send a message but i will not see it
            {
                var SenderId = Context.UserIdentifier;
                var mems = await _chatMembersService.GetAllByChatId(model.ChatId);
                var RecivId = mems.Where(m => m.MemberId != SenderId).Select(m => m.MemberId).FirstOrDefault();

                if (await _blockingService.IsBlocked(RecivId, SenderId)) return;
            }
            else if (chat.chatType == Helpers.Enums.ChatType.grp)
            {
                var SenderId = Context.UserIdentifier;
                var mem = await _chatMembersService.GetByChatIdAndMemberId(model.ChatId,SenderId);
                if (mem is null) return;
            }
             * *************************************************
            */

            var userId = Context.UserIdentifier;
            var user = await _accountService.FindById(userId);
            
            
            // ********************
            var res = await _messageService.Create(model, files);
            if (!res.IsSuccess) return;
            var message = res.Object;
            var chatMembers = await _chatMembersService.GetAllByChatId(model.ChatId); // all chat members
            foreach(var chatMember in chatMembers)
            {
                if (chatMember.MemberId == Context.UserIdentifier) continue; 

                var recipient = await _accountService.FindById(chatMember.MemberId);
                if(await _presenseTrackerService.IsUserOnline(chatMember.MemberId))
                {
                    await Clients.Group(model.ChatId.ToString()).SendAsync("ReceiveMssage", 
                        new { SenderPhoneNumber = recipient.PhoneNumber, Message=model.Content, ChatId = model.ChatId, MessageStatus = MessageStatusEnum.delivered, CreatedAt=DateTime.UtcNow });
                    // create message status 
                    message.messageStatuses.Add(new MessageStatus() { MessageId=message.Id, RecieverId=chatMember.MemberId, status=MessageStatusEnum.delivered, UpdatedAt=DateTime.UtcNow});
                }
                else
                {
                    // FCM
                    var pushNotf = new CreatePushNotificationDTO() { Content = model.Content , Title="New Message"};
                    var resp = await _notificationService.PushNotification(chatMember.MemberId, pushNotf);
                    if (resp) message.messageStatuses.Add(new MessageStatus() { MessageId = message.Id, RecieverId = chatMember.MemberId, status = MessageStatusEnum.sent, UpdatedAt = DateTime.UtcNow });
                }
            }
            // *******************
            
            
            //await Clients.Group(ChatId.ToString()).ReceiveMessage(user.UserName,msg);
            await _messageService.Create(new CreateMessageDTO() { Content = model.Content, ChatId = model.ChatId, UserId = userId }, files);
            await Clients.Group(model.ChatId.ToString()).SendAsync("ReceiveMessage", user.UserName, model.Content);
            _logger.LogInformation("Sending message from user '{DisplayName}' is succedded", user.DisplayName);
        }
        
        public async Task DeleteMessage(Guid MessageId, bool IsForEveryone)
        {
            var message = await _messageService.GetById(MessageId);
            var UserId = Context.UserIdentifier;
            await Clients.Group(message.ChatId.ToString()).SendAsync("DeleteMessage", new { MessageId, UserId, IsForEveryone });
        }







        public async Task AddUserToGroup(Guid GroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString());
        }
        public async Task RemoveUserFromGorup(Guid groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        }








        public async Task SendReaction(Guid MessageId, string reactType)
        {
            var message = await _messageService.GetById(MessageId);
            var chat = await _chatService.GetById(message.ChatId);
            if (chat.chatType == Helpers.Enums.ChatType.prv)
            {
                var SenderId = Context.UserIdentifier;
                var mems = await _chatMembersService.GetAllByChatId(message.ChatId);
                var RecivId = mems.Where(m => m.MemberId != SenderId).Select(m => m.MemberId).FirstOrDefault();

                if (await _blockingService.IsBlocked(RecivId, SenderId)) return;
            }
            else if (chat.chatType == Helpers.Enums.ChatType.grp)
            {
                var SenderId = Context.UserIdentifier;
                var mem = await _chatMembersService.GetByChatIdAndMemberId(message.ChatId, SenderId);
                if (mem is null) return;
            }
            var UserId = Context.UserIdentifier;
            await _reactionService.Create(new CreateReactionDTO() { ReacterId=UserId, MessageId=MessageId});
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveReaction", new {MessageId, UserId, reactType });
        }
        public async Task DeleteReaction(Guid MessageId, Guid ReactionId)
        {
            var message = await _messageService.GetById(MessageId);
            var UserId = Context.UserIdentifier;
            await _reactionService.Delete(ReactionId);
            await Clients.Group(message.ChatId.ToString()).SendAsync("DeleteReaction", new { MessageId, UserId, ReactionId });
        }










        public async Task SendStartTypingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;
            
            await Clients.GroupExcept(ChatId.ToString(), new [] {userId}).SendAsync("StartTyping",new {UserId=userId, ChatId=ChatId});
        }
        public async Task SendStopTypingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StopTyping", new { UserId = userId, ChatId = ChatId});
        }
        public async Task SendStartRecordingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StartRecording", new { UserId = userId, ChatId = ChatId});
        }
        public async Task SendStopRecordingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StopRecording", new { UserId = userId, ChatId = ChatId });
        }










        public async Task ChangeMessageStatusofChatToSeen(Guid ChatId) 
        {
            var chat = await _chatService.GetById(ChatId);
            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            if(chat.chatType == ChatType.prv)
            {
                var setngs = await _userSettingsService.GetByUserId(UserId);
                if (setngs.ReadReceiptsEnabled) return;
            }

            var DeliveredMsgs = await _messageService.GetAllAfterDatetimeWithChatId(user.LastSeen, ChatId);
            foreach (var msg in DeliveredMsgs)
            {
                // difference between Now & UtcNow ???
                msg.messageStatuses.Add(new MessageStatus() { MessageId = msg.Id, RecieverId = UserId, status = MessageStatusEnum.seen, UpdatedAt = DateTime.UtcNow });
                await Clients.GroupExcept(msg.ChatId.ToString(), new[] { UserId }).SendAsync("ChangeMessageStatusToSeen", msg.Id);
            }
        }
        public async Task ChangeMessageStatusofChatToDelivered(Guid ChatId)
        {
            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            var DeliveredMsgs = await _messageService.GetAllAfterDatetimeWithChatId(user.LastSeen, ChatId);
            foreach (var msg in DeliveredMsgs)
            {
                // difference between Now & UtcNow ???
                msg.messageStatuses.Add(new MessageStatus() { MessageId = msg.Id, RecieverId = UserId, status = MessageStatusEnum.delivered, UpdatedAt = DateTime.UtcNow });
                await Clients.GroupExcept(msg.ChatId.ToString(), new[] { UserId }).SendAsync("ChangeMessageStatusToSeen", msg.Id);
            }
        }










        public override async Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier;
            await _presenseTrackerService.UserConnectedAsync(UserId, Context.ConnectionId);


            var user = await _accountService.FindById(UserId);
            /*user.IsOnline = true;
            await _accountService.SaveChangesAsync(user);

            await _userConnectionService.Create(new CreateUserConnectionDTO() { UserId = UserId ,ConnectionId = Context.ConnectionId });
            */

            var UnSeenMessages = await _messageService.GetAllAfterDatetime(user.LastSeen);
            foreach(var msg in UnSeenMessages)
            {
                // difference between Now & UtcNow ???
                msg.messageStatuses.Add(new MessageStatus() { MessageId = msg.Id, RecieverId = UserId, status = MessageStatusEnum.delivered, UpdatedAt = DateTime.UtcNow });
                await Clients.GroupExcept(msg.ChatId.ToString(), new[] { UserId }).SendAsync("ChangeMessageStatusToDelivered", msg.Id);
            }

            var groups = await _chatMembersService.GetAllByUserId(UserId);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.ChatId.ToString());
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _userConnectionService.Delete(Context.ConnectionId);

            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            /*if (!user.UserConnections.Any())  // if the user dont open the app in another device
            {
                user.IsOnline = false;
                user.LastSeen = DateTime.UtcNow;
                await _accountService.SaveChangesAsync(user);
            }*/

            await _presenseTrackerService.UserDisconnectedAsync(UserId, Context.ConnectionId);
            bool isStillOnline = await _presenseTrackerService.IsUserOnline(UserId);
            if (!isStillOnline)
            {
                await Clients.Others.SendAsync("UserIsOffline", UserId);
                user.LastSeen = DateTime.UtcNow;
                user.IsOnline = false;
                await _accountService.SaveChangesAsync(user);
            }

            // if user open the app in another device, he is still online
            await base.OnDisconnectedAsync(exception);
        }
    }
}
