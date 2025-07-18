//-----------------------------------------------------------------------
// This file is autogenerated by EntityCore
// <auto-generated />
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Services.Answers;
using Common.Paginations.Models;
using Common;
using DataTransferObjects.Answers;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswersService _answersService;
        public AnswersController(IAnswersService answersService)
        {
            _answersService = answersService;
        }

        [HttpPost]
        public async Task<Result<AnswerViewModel>> AddAsync(AnswerCreationDto answerCreationDto)
        {
            return Result<AnswerViewModel>.Success(await _answersService.AddAsync(answerCreationDto));
        }

        [HttpGet]
        public async Task<Result<List<AnswerViewModel>>> GetAllAsync()
        {
            return Result<List<AnswerViewModel>>.Success(await _answersService.GetAllAsync());
        }

        [HttpPost("filter")]
        public async Task<Result<ListResult<AnswerViewModel>>> FilterAsync(PaginationOptions filter)
        {
            return Result<ListResult<AnswerViewModel>>.Success(await _answersService.FilterAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<Result<AnswerViewModel>> GetByIdAsync(int id)
        {
            return Result<AnswerViewModel>.Success(await _answersService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<Result<AnswerViewModel>> UpdateAsync(int id, AnswerModificationDto answerModificationDto)
        {
            return Result<AnswerViewModel>.Success(await _answersService.UpdateAsync(id, answerModificationDto));
        }

        [HttpDelete("{id}")]
        public async Task<Result<AnswerViewModel>> DeleteAsync(int id)
        {
            return Result<AnswerViewModel>.Success(await _answersService.DeleteAsync(id));
        }
    }
}