namespace Gamification.Domain.Common
{
    public class Auditable<T> : Entity<T>
    {
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public class Auditable : Auditable<long>
    {
    }
}
