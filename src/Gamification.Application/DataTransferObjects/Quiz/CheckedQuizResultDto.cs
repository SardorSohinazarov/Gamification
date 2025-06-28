namespace Gamification.Application.DataTransferObjects.Quiz
{
    public class CheckedQuizResultDto
    {
        public int QuestionsCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public double Score => QuestionsCount == 0 
                ? 0 
                : CorrectAnswersCount * 100 / QuestionsCount;
    }
}
