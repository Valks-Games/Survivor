public static class StringExtention
{
    public static string ToTitleCase(this string s)
    {
        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
    }
}