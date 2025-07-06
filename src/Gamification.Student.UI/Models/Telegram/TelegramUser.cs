namespace Gamification.Student.UI.Models.Telegram
{
    public class WebAppInitData
    {
        public string? QueryId { get; set; }
        public WebAppUser? User { get; set; }
        public WebAppUser? Receiver { get; set; }
        public WebAppChat? Chat { get; set; }
        public string? ChatType { get; set; }
        public string? ChatInstance { get; set; }
        public string? StartParam { get; set; }
        public int? CanSendAfter { get; set; }
        public long AuthDate { get; set; }
        public string Hash { get; set; } = default!;
        public string? Signature { get; set; }
    }

    public class WebAppUser
    {
        public long Id { get; set; }
        public bool IsBot { get; set; }
        public string FirstName { get; set; } = default!;
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? LanguageCode { get; set; }
        public string? PhotoUrl { get; set; }
    }

    public class WebAppChat
    {
        public long Id { get; set; }
        public string Type { get; set; } = default!;
        public string? Title { get; set; }
        public string? Username { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
