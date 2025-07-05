namespace Gamification.Application.Models
{
    public class TelegramAuthData
    {
        public TelegramUser User { get; set; }
        public string Auth_date { get; set; }
        public string Hash { get; set; }
        public string Query_id { get; set; } // optional
        public string Start_param { get; set; } // optional
    }
}
