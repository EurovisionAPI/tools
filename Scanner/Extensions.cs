namespace Scanner;

internal static class Extensions
{
    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
    {
        return source != null && !source.Any();
    }

    public static string IsoCountryCodeToFlagEmoji(this string country)
    {
        return string.Concat(country.ToUpper().Select(x => char.ConvertFromUtf32(x + 0x1F1A5)));
    }
}
