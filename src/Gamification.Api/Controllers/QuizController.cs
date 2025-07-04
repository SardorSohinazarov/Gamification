using Common;
using DataTransferObjects.Questions;
using DataTransferObjects.Tests;
using Gamification.Application.DataTransferObjects.Quiz;
using Gamification.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Questions;
using Services.Tests;

namespace Gamification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly GamificationDb _gamificationDb;
        private readonly IQuestionsService _questionsService;
        private readonly ITestsService _testsService;

        public QuizController(
            GamificationDb gamificationDb,
            IQuestionsService questionsService,
            ITestsService testsService)
        {
            _gamificationDb = gamificationDb;
            _questionsService = questionsService;
            _testsService = testsService;
        }

        [HttpGet("test/{testId}")]
        public async Task<Result<List<QuestionViewModel>>> GetAllByTestAsync(int testId)
        {
            return Result<List<QuestionViewModel>>.Success(await _questionsService.GetAllByTestAsync(testId));
        }

        [HttpPost("check")]
        public async Task<Result<CheckedQuizResultDto>> CheckTest([FromBody] QuizResultDto quizResultDto)
        {
            var test = _gamificationDb.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == quizResultDto.QuizId);

            if (test is null)
            {
                throw new Exception($"Test with ID {quizResultDto.QuizId} not found.");
            }

            CheckedQuizResultDto checkedQuizResultDto = new CheckedQuizResultDto
            {
                CorrectAnswersCount = 0,
                QuestionsCount = test.Questions.Count()
            };

            foreach (var quizResult in quizResultDto.Answers)
            {
                var question = test.Questions.FirstOrDefault(q => q.Id == quizResult.Key);
                if (question is null)
                {
                    throw new Exception($"Question with ID {quizResult.Key} not found in the test.");
                }

                var isTrue = question.Answers.Any(a => a.IsCorrect && quizResult.Value.Contains(a.Id));

                if (isTrue)
                {
                    checkedQuizResultDto.CorrectAnswersCount ++;
                }
            }

            return Result<CheckedQuizResultDto>.Success(checkedQuizResultDto);
        }

        [HttpGet("tests")]
        public async Task<Result<List<TestViewModel>>> GetAllAsync()
        {
            return Result<List<TestViewModel>>.Success(await _testsService.GetAllAsync());
        }
    }
}
