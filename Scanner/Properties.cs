namespace Scanner;

public static class Properties
{
    private const string ARGUMENT_PREFFIX = "-";

    private const string INPUT_PATH_ARGUMENT = "input";
    public static string INPUT_PATH { get; private set; }

    private const string README_PATH_ARGUMENT = "readme";
    public static string README_PATH { get; private set; }

    private const string IS_JUNIOR_ARGUMENT = "junior";
    public static bool IS_JUNIOR { get; private set; }

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

                case IS_JUNIOR_ARGUMENT:
                    IS_JUNIOR = true;
                    break;

                case README_PATH_ARGUMENT:
                    README_PATH = arguments[++i];
                    break;
            }
        }
    }
}
