using Common;
using Gamification.Student.UI.Models.Quiz;
using Gamification.Student.UI.Models.Tests;
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
                var response = await _httpClient.GetAsync($"api/quiz/test/{testId}");
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

        public async Task<CheckedQuizResultDto> CheckTestAsync(QuizResultDto quizResultDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/quiz/check", quizResultDto);
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<Result<CheckedQuizResultDto>>();
                    if (result != null && result.Succeeded && result.Data != null)
                    {
                        return result.Data;
                    }
                    else
                    {
                        throw new Exception($"Failed to check test, message:{result.Message}");
                    }
                }
                else
                {
                    throw new Exception("Failed to check test");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TestViewModel>> GetTestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/quiz/tests");

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<Result<List<TestViewModel>>>();

                    if (result != null && result.Succeeded && result.Data != null)
                    {
                        return result.Data;
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve tests, message:{result.Message}");
                    }
                }
                else
                {
                    throw new Exception("Failed to load tests");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
