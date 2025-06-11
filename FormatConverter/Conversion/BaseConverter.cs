using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace FormatConverter.Conversion;

internal abstract class BaseConverter
{
    protected const string CONTEST_FILE_NAME = "contest";
    protected const string CONTESTANTS_FOLDER_NAME = "contestants";
    protected const string CONTESTANT_FILE_NAME = "contestant";
    protected const string LYRICS_FOLDER_NAME = "lyrics";
    protected const string ROUNDS_FOLDER_NAME = "rounds";
    protected const string FILE_NAME_SEPARATOR = "_";
    protected const char LANGUAGE_SEPARATOR = ',';
    protected const string LYRICS_PARTS_SEPARATOR = "\n\n";
    protected static readonly Encoding DATASET_ENCODING = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    
    protected static readonly JsonSerializerOptions SCRAPER_JSON_OPTIONS = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected static readonly JsonSerializerOptions DATASET_JSON_OPTIONS = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
