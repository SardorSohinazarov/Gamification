using DataTransferObjects.Questions;
using Gamification.Application.DataTransferObjects.Quiz;

namespace Gamification.Student.UI.Services.Quiz
{
    public interface IQuizService
    {
        Task<List<QuestionViewModel>> GetQuestionsAsync(int testId);
        Task<CheckedQuizResultDto> CheckTestAsync(QuizResultDto quizResultDto);
    }
}
