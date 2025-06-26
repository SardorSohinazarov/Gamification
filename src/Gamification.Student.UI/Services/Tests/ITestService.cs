using DataTransferObjects.Tests;

namespace Gamification.Student.UI.Services.Tests
{
    public interface ITestService
    {
        Task<List<TestViewModel>> GetTestsAsync();
    }
}
