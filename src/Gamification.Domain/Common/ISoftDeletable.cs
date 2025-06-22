namespace Gamification.Domain.Common
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
