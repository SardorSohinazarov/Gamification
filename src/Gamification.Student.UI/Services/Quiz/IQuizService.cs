using Common;
using DataTransferObjects.Questions;
using Gamification.Domain.Entities;

namespace Gamification.Student.UI.Services.Quiz
{
    public interface IQuizService
    {
        Task<List<QuestionViewModel>> GetQuestionsAsync(int testId);
    }
}
