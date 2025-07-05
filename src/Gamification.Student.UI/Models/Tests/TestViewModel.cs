using System.Text.Json.Serialization;

namespace Gamification.Student.UI.Models.Tests
{
    public class TestViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("duration")]
        public float Duration { get; set; }
        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }
    }

    public class QuestionViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("answers")]
        public List<AnswerViewModel> Answers { get; set; }
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }
        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }
    }

    public class AnswerViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }
        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }
    }
}
