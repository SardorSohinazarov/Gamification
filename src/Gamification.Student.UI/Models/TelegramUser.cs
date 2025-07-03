namespace Gamification.Student.UI.Models
{
    public class TelegramUser
    {
        public long Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Username { get; set; }
        public string Photo {
            get
            {
                return $"https://t.me/i/userpic/320/{Username}.jpg";
            } }
        public string Language_code { get; set; }
    }
}
