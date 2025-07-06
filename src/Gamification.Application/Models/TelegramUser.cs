namespace Gamification.Application.Models
{
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

        public string? initData { get; set; }
    }
}
