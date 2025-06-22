using Gamification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure
{
    public partial class GamificationDb : DbContext
    {
        // tables
        public DbSet<Book> Books { get; set; }
    }
}
