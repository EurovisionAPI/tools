namespace Scanner;

internal static class Extensions
{
    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
    {
        return source != null && source.Any();
    }
}
