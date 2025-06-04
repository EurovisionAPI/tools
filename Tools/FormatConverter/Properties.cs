using FormatConverter.Conversion;

namespace FormatConverter;

public static class Properties
{
    private const string ARGUMENT_PREFFIX = "-";

    private const string INPUT_PATH_ARGUMENT = "input";
    public static string INPUT_PATH { get; private set; }

    private const string OUTPUT_PATH_ARGUMENT = "output";
    public static string OUTPUT_PATH { get; private set; }

    private const string MODE_ARGUMENT = "mode";
    public static ConvertionMode MODE { get; private set; }

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

                case OUTPUT_PATH_ARGUMENT:
                    OUTPUT_PATH = arguments[++i];
                    break;

                case MODE_ARGUMENT:
                    MODE = (ConvertionMode)int.Parse(arguments[++i]);
                    break;
            }
        }
    }
}
