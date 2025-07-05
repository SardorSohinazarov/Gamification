using Gamification.Student.UI.Models.Quiz;
using Gamification.Student.UI.Models.Tests;

namespace Gamification.Student.UI.Services.Quiz
{
    public interface IQuizService
    {
        Task<List<QuestionViewModel>> GetQuestionsAsync(int testId);
        Task<CheckedQuizResultDto> CheckTestAsync(QuizResultDto quizResultDto);
        Task<List<TestViewModel>> GetTestsAsync();
    }
}
