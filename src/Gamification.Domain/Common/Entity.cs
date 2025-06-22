namespace Gamification.Domain.Common
{
    public class Entity<T>
    {
        public T Id { get; set; }
    }

    public class Entity : Entity<long>
    {
    }
}
