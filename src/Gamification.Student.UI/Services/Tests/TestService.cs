using Common;
using DataTransferObjects.Tests;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gamification.Student.UI.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly HttpClient _httpClient;
        public TestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TestViewModel>> GetTestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/tests");

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
