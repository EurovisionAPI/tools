using System.Diagnostics;
using System.Globalization;
using FormatConverter.Conversion;

namespace FormatConverter;

internal class Program
{
    static async Task Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        Properties.ReadArguments(args);

        Console.WriteLine("Start conversion");
        Stopwatch stopwatch = Stopwatch.StartNew();

        if (Properties.MODE == ConvertionMode.ToMachine)
        {
            ToScraperConverter converter = new ToScraperConverter();
            converter.Convert(Properties.INPUT_PATH, Properties.OUTPUT_PATH);
        }
        else
        {
            ToDatasetConverter converter = new ToDatasetConverter();
            await converter.ConvertAsync(Properties.INPUT_PATH, Properties.OUTPUT_PATH);
        }

        Console.WriteLine("Conversion completed, duration: " + stopwatch.Elapsed);
    }
}