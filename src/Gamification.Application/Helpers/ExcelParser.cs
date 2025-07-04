using ClosedXML.Excel;
using Common.ServiceAttribute;
using Gamification.Application.DataTransferObjects.Tests;
using Gamification.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Gamification.Application.Helpers
{
    [ScopedService]
    public class ExcelParser
    {
        private readonly ILogger<ExcelParser> _log;

        public ExcelParser(ILogger<ExcelParser> log)
            => _log = log;

        public async Task<Test> ImportAsync(TestFileCreationDto testFileCreationDto, CancellationToken ct = default)
        {
            if (testFileCreationDto == null || testFileCreationDto.File.Length == 0)
                throw new ArgumentException("Fayl yuborilmagan yoki bo‘sh.", nameof(testFileCreationDto));

            using var stream = testFileCreationDto.File.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var ws = workbook.Worksheet(1);

            var testTitle = ws.Cell(1, 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(testTitle))
                throw new InvalidDataException("A1 katak bo‘sh – test nomi topilmadi.");

            var test = new Test
            {
                Title = testTitle,
                Description = string.Empty,
                Duration = 1f,
                Status = TestStatus.Public,
                Questions = new List<Question>()
            };

            var row = 2;
            while (!ws.Cell(row, 1).IsEmpty())
            {
                var questionText = ws.Cell(row, 1).GetString().Trim();
                if (string.IsNullOrWhiteSpace(questionText))
                {
                    _log.LogWarning("Qator {Row} da savol matni bo‘sh – o‘tkazib yuborildi.", row);
                    row++;
                    continue;
                }

                var question = new Question
                {
                    Text = questionText,
                    Answers = new List<Answer>()
                };

                // --- B ustun: to‘g‘ri javob ---
                var correct = ws.Cell(row, 2).GetString().Trim();
                if (string.IsNullOrWhiteSpace(correct))
                    throw new InvalidDataException($"Qator {row}: to‘g‘ri javob (B{row}) bo‘sh.");

                question.Answers.Add(new Answer
                {
                    Text = correct,
                    IsCorrect = true
                });

                // --- C va undan keyin: noto‘g‘ri javoblar ---
                var col = 3;
                while (!ws.Cell(row, col).IsEmpty())
                {
                    var wrong = ws.Cell(row, col).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(wrong))
                    {
                        question.Answers.Add(new Answer
                        {
                            Text = wrong,
                            IsCorrect = false
                        });
                    }
                    col++;
                }

                test.Questions.Add(question);
                row++;
            }

            _log.LogInformation("Excel fayldan Test #{TestId} ('{Title}') import qilindi.",
                                test.Id, test.Title);
            return test;
        }
    }
}
