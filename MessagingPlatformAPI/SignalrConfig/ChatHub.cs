using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Implementation;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.SignalrConfig
{
    public class ChatHub : Hub//<IChatMethod>
    {
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IChatMembersService _chatMembersService;
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(IAccountService accountService, IChatMembersService chatMembersService, ILogger<ChatHub> logger, IUserConnectionService userConnectionService, IMessageService messageService)
        {
            _accountService = accountService;
            _chatMembersService = chatMembersService;
            _logger = logger;
            _userConnectionService = userConnectionService;
            _messageService = messageService;
        }
        public async Task SendMessage(string msg, Guid ChatId)
        {
            //await Clients.Group(ChatId.ToString()).ReceiveMessage(user.UserName,msg);
            var userId = Context.UserIdentifier;
            var user = await _accountService.FindById(userId);
            await Clients.Group(ChatId.ToString()).SendAsync("ReceiveMessage", user.UserName, msg);
            await _messageService.Create(new CreateMessageDTO() { Content = msg, ChatId = ChatId, UserId = userId });
            _logger.LogInformation("Sending message from user '{fname} {lname}' is succedded", user.FirstName, user.LastName);
        }
        public async Task AddUserToGroup(Guid GroupId)
        {
            var user = await _accountService.FindById(Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupId.ToString());
        }



        public async Task StartTypingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;
            
            await Clients.GroupExcept(ChatId.ToString(), new [] {userId}).SendAsync("StartTyping",new {UserId=userId, ChatId=ChatId, IsTyping=true});
        }
        public async Task StopTypingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StopTyping", new { UserId = userId, ChatId = ChatId, IsTyping = false });
        }
        public async Task StartRecordingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StartRecording", new { UserId = userId, ChatId = ChatId, IsRecording = true });
        }
        public async Task StopRecordingIndicator(Guid ChatId)
        {
            var userId = Context.UserIdentifier;

            await Clients.GroupExcept(ChatId.ToString(), new[] { userId }).SendAsync("StopRecording", new { UserId = userId, ChatId = ChatId, IsRecording = false });
        }



        public override async Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier;
            var user = await _accountService.FindById(UserId);
            user.IsOnline = true;
            await _accountService.SaveChangesAsync(user);

            await _userConnectionService.Create(new CreateUserConnectionDTO() { UserId = UserId ,ConnectionId = Context.ConnectionId });

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
