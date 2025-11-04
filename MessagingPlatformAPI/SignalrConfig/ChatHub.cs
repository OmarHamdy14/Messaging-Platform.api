using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
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
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(IAccountService accountService, IChatMembersService chatMembersService, ILogger<ChatHub> logger, IUserConnectionService userConnectionService, IMessageService messageService, IChatService chatService, IBlockingService blockingService)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
            _logger = logger;
            _userConnectionService = userConnectionService;
            _messageService = messageService;
            _chatService = chatService;
            _blockingService = blockingService;
        }
        public async Task SendMessage(string msg, Guid ChatId, List<IFormFile> files)
        {
            var chat = await _chatService.GetById(ChatId);
            if (chat.chatType == Helpers.Enums.ChatType.prv)
            {
                var SenderId = Context.UserIdentifier;
                var mems = await _chatMembersService.GetAllByChatId(ChatId);
                var RecivId = mems.Where(m => m.MemberId != SenderId).Select(m => m.MemberId).FirstOrDefault();

                if (await _blockingService.IsBlocked(RecivId, SenderId)) return;
            }
            if (chat.chatType == Helpers.Enums.ChatType.grp)
            {
                var SenderId = Context.UserIdentifier;
                var mem = await _chatMembersService.GetByChatIdAndMemberId(ChatId,SenderId);
                if (mem is null) return;
            }
            //await Clients.Group(ChatId.ToString()).ReceiveMessage(user.UserName,msg);
            var userId = Context.UserIdentifier;
            var user = await _accountService.FindById(userId);
            await _messageService.Create(new CreateMessageDTO() { Content = msg, ChatId = ChatId, UserId = userId }, files);
            await Clients.Group(ChatId.ToString()).SendAsync("ReceiveMessage", user.UserName, msg);
            _logger.LogInformation("Sending message from user '{fname} {lname}' is succedded", user.FirstName, user.LastName);
        }


        public async Task AddUserToGroup(Guid GroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString());
        }
        public async Task RemoveUserFromGorup(Guid groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
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
            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
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
            var user = await _accountService.FindById(UserId);
            user.IsOnline = true;
            await _accountService.SaveChangesAsync(user);

            await _userConnectionService.Create(new CreateUserConnectionDTO() { UserId = UserId ,ConnectionId = Context.ConnectionId });

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
            if (!user.UserConnections.Any())  // if the user dont open the app in another device
            {
                user.IsOnline = false;
                user.LastSeen = DateTime.UtcNow;
                await _accountService.SaveChangesAsync(user);
            }
            // if user open the app in another device, he is still online
            await base.OnDisconnectedAsync(exception);
        }
    }
}
