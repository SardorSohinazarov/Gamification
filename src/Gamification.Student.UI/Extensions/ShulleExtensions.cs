namespace Gamification.Student.UI.Extensions
{
    public static class ShulleExtensions
    {
        public static T[] Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).ToArray();
        }
    }
}
