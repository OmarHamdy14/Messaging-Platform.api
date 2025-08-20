namespace MessagingPlatformAPI.Helpers.DTOs.UsersGroupDTOs
{
    public class RemoveUserFromGroupDTO
    {
        public string UserId { get; set; }
        public Guid ChatGroupId { get; set; }
    }
}
