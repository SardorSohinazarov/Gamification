using Common;
using DataTransferObjects.Questions;
using System.Net.Http.Json;

namespace Gamification.Student.UI.Services.Quiz
{
    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;
        public QuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<QuestionViewModel>> GetQuestionsAsync(int testId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/questions/test/{testId}");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<Result<List<QuestionViewModel>>>();
                    if (result != null && result.Succeeded && result.Data != null)
                    {
                        return result.Data;
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve questions, message:{result.Message}");
                    }
                }
                else
                {
                    throw new Exception("Failed to load questions");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
