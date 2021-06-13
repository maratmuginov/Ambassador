using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ambassador.Lib.Helpers
{
    public static class JsonHelper
    {
        public static T DeserializeFile<T>(string filePath)
        {
            string utf8Json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(utf8Json);
        }

        public static async Task<T> DeserializeFileAsync<T>(string filePath)
        {
            await using var utf8Json = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(utf8Json);
        }
    }
}
