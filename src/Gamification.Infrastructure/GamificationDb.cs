using Gamification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure
{
    public partial class GamificationDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonTest> LessonTests { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
    }
}
