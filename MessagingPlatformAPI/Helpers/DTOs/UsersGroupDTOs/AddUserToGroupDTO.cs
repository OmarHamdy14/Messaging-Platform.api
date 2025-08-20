namespace MessagingPlatformAPI.Helpers.DTOs.UsersGroupDTOs
{
    public class AddUserToGroupDTO
    {
        public string UserId { get; set; }
        public Guid ChatGroupId { get; set; }
    }
}
