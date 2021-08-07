namespace niroj.website.Helpers
{
    public class Alert
    {
        public string message { get; set; }
        public string message_type { get; set; }
        
    }
    public enum MessageType
    {
        success,
        error,
        info
    }
}
