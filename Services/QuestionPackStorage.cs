using ITHS_LABB_2_Quiz_Configurator.Models;
using System.IO;
using System.Text.Json;

namespace ITHS_LABB_2_Quiz_Configurator.Services
{
    class QuestionPackStorage
    {
        public static async Task SavePacksToJsonFile(List<QuestionPack> packs, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath)
                ?? throw new ArgumentException("The file path must include a directory", nameof(filePath));

            Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(packs);
            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task<List<QuestionPack>> LoadPacksFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return [];
            }

            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<QuestionPack>>(json) ?? new List<QuestionPack>();
        }
    }
}
