namespace Scanner;

public static class Properties
{
    private const string ARGUMENT_PREFFIX = "-";

    private const string INPUT_PATH_ARGUMENT = "input";
    public static string INPUT_PATH { get; private set; }

    private const string COUNTRIES_FILENAME_ARGUMENT = "countries";
    public static string COUNTRIES_FILENAME { get; private set; } = "countries";

    private const string JUNIOR_FILENAME_ARGUMENT = "junior";
    public static string JUNIOR_FILENAME { get; private set; } = "junior";

    private const string JUNIOR_PATTERN_ARGUMENT = "junior_pattern";
    public static string JUNIOR_PATTERN { get; private set; } = "Junior";

    private const string SENIOR_FILENAME_ARGUMENT = "senior";
    public static string SENIOR_FILENAME { get; private set; } = "senior";

    private const string SENIOR_PATTERN_ARGUMENT = "senior_pattern";
    public static string SENIOR_PATTERN { get; private set; } = "Senior";

    private const string README_PATH_ARGUMENT = "readme";
    public static string README_PATH { get; private set; }


    public static void ReadArguments(string[] arguments)
    {
        for (int i = 0; i < arguments.Length; i++)
        {
            string command = arguments[i];

            if (!command.StartsWith(ARGUMENT_PREFFIX))
                throw new ArgumentException($"Arguments must start with {ARGUMENT_PREFFIX}");

            switch (command.Substring(ARGUMENT_PREFFIX.Length).ToLower())
            {
                case INPUT_PATH_ARGUMENT:
                    INPUT_PATH = arguments[++i];
                    break;

                case COUNTRIES_FILENAME_ARGUMENT:
                    COUNTRIES_FILENAME = arguments[++i];
                    break;

                case JUNIOR_FILENAME_ARGUMENT:
                    JUNIOR_FILENAME = arguments[++i];
                    break;

                case JUNIOR_PATTERN_ARGUMENT:
                    JUNIOR_PATTERN = arguments[++i];
                    break;

                case SENIOR_PATTERN_ARGUMENT:
                    SENIOR_PATTERN = arguments[++i];
                    break;

                case SENIOR_FILENAME_ARGUMENT:
                    SENIOR_FILENAME = arguments[++i];
                    break;

                case README_PATH_ARGUMENT:
                    README_PATH = arguments[++i];
                    break;
            }
        }
    }
}
