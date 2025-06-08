namespace FormatConverter.Utilities;

public static class Extensions
{
    public static bool SafeSequenceEqual<T>(this IEnumerable<T>? first, IEnumerable<T>? second)
    {
        if (first is null && second is null) return true;
        if (first is null || second is null) return false;

        return first.SequenceEqual(second);
    }
}
