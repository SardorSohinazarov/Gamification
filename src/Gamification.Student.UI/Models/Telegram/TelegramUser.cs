namespace Gamification.Student.UI.Models.Telegram
{
    public class WebAppInitData
    {
        public string? query_id { get; set; }
        public WebAppUser? user { get; set; }
        public WebAppUser? receiver { get; set; }
        public WebAppChat? chat { get; set; }
        public string? chat_type { get; set; }
        public string? chat_instance { get; set; }
        public string? start_param { get; set; }
        public int? can_send_after { get; set; }
        public string? auth_date { get; set; }
        public string hash { get; set; } = default!;
        public string? signature { get; set; }
    }

    public class WebAppUser
    {
        public long id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; } = default!;
        public string? last_name { get; set; }
        public string? username { get; set; }
        public string? language_code { get; set; }
        public bool? is_premium { get; set; }
        public bool? added_to_attachment_menu { get; set; }
        public bool? allows_write_to_pm { get; set; }
        public string? photo_url { get; set; }
    }

    public class WebAppChat
    {
        public long id { get; set; }
        public string type { get; set; } = default!;
        public string? title { get; set; }
        public string? username { get; set; }
        public string? photo_url { get; set; }
    }
}
