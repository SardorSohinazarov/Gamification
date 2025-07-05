using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gamification.Application.DataTransferObjects.Tests
{
    public class TestFileCreationDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
