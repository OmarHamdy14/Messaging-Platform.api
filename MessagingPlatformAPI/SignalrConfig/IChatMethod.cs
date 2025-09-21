namespace MessagingPlatformAPI.SignalrConfig
{
    public interface IChatMethod
    {
        Task ReceiveMessage(string name, string msg);
    }
}
