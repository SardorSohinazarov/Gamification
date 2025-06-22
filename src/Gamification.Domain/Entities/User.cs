using Gamification.Domain.Common;

namespace Gamification.Domain.Entities
{
    public class User : Auditable<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public long TelegramId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public UserType UserType { get; set; }
    }

    public class Course : Auditable<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // 1.5 soat davom etadi
        public float Duration { get; set; }
        public TimeOnly StartTime { get; set; }

        public DateOnly StartDate { get; set; }
    }

    public class Lesson : Auditable<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Test : Auditable<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // 1.5 soat davom etadi
        public float Duration { get; set; }
        public DateOnly StartAt { get; set; }

        public TestStatus Status { get; set; }
    }

    public class Question : Auditable<int>
    {
        public string Text { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }

    public class Answer : Auditable<int>
    {
        public string Text { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class UserAnswer : Auditable<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public DateTime AnsweredAt { get; set; }
    }
}
