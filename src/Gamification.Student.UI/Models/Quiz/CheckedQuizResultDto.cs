using System.Text.Json.Serialization;

namespace Gamification.Student.UI.Models.Quiz
{
    public class CheckedQuizResultDto
    {
        [JsonPropertyName("questionsCount")]
        public int QuestionsCount { get; set; }
        [JsonPropertyName("correctAnswersCount")]
        public int CorrectAnswersCount { get; set; }

        public double Score => QuestionsCount == 0
                ? 0
                : CorrectAnswersCount * 100 / QuestionsCount;
    }
}
