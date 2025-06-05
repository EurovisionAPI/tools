using System.Diagnostics;
using System.Globalization;
using FormatConverter.Conversion;

namespace FormatConverter;

internal class Program
{
    static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        Properties.ReadArguments(args);

        Stopwatch stopwatch = Stopwatch.StartNew();

        ToScraperConverter converter = new ToScraperConverter();
        converter.Convert(Properties.INPUT_PATH, Properties.OUTPUT_PATH);

        /*
        ToDatasetConverter converter = new ToDatasetConverter();
        //transformer.ToDatasetFormat("eurovision.json", "senior");
        await converter.ConvertAsync("junior.json", "junior");*/

        Console.WriteLine(stopwatch.Elapsed);
    }
}