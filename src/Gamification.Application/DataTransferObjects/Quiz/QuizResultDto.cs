namespace Gamification.Application.DataTransferObjects.Quiz
{
    public class QuizResultDto
    {
        public int QuizId { get; set; }
        public Dictionary<int, List<int>> Answers { get; set; } = new();
    }
}
