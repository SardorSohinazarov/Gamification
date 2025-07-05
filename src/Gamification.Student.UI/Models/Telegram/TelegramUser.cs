namespace Gamification.Student.UI.Models.Telegram
{
    public class TelegramUser
    {
        public long Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Username { get; set; }
        public string Photo
        {
            get
            {
                if (Username is not null)
                {
                    return $"https://t.me/i/userpic/320/{Username}.jpg";
                }
                return null;
            }
        }
        public string Language_code { get; set; }
    }
}
