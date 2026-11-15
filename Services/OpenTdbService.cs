using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace ITHS_LABB_2_Quiz_Configurator.Services
{
    internal class OpenTdbService
    {
        private static readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://opentdb.com")
        };

        public static async Task<List<Question>> GetQuestionsAsync(ImportOptions options)
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"/api.php?amount={options.QuestionCount}&category={options.CategoryId}&difficulty={options.SelectedDifficulty.ToString().ToLowerInvariant()}&type=multiple");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            JsonElement root = jsonDoc.RootElement;
            int responseCode = root.GetProperty("response_code").GetInt32();

            if (responseCode != 0)
            {
                string message = responseCode switch
                {
                    1 => "No questions found. Try another category, difficulty or question count.",
                    2 => "Invalid import settings.",
                    3 => "OpenTDB session token not found.",
                    4 => "OpenTDB session token is exhausted.",
                    5 => "Too many requests. Please wait a few seconds and try again.",
                    _ => $"OpenTDB returned an unknown error (code {responseCode})."
                };

                throw new InvalidOperationException(message);
            }

            JsonElement.ArrayEnumerator results = root.GetProperty("results").EnumerateArray();

            var questions = new List<Question>();

            foreach (var result in results)
            {
                string query = WebUtility.HtmlDecode(
                     result.GetProperty("question").GetString() ?? ""
                 );
                string correctAnswer = WebUtility.HtmlDecode(result.GetProperty("correct_answer").GetString() ?? "");

                var incorrectAnswers = new List<string>();
                foreach (JsonElement answer in result.GetProperty("incorrect_answers").EnumerateArray())
                {
                    var decodedAnswer = WebUtility.HtmlDecode(answer.GetString() ?? "");
                    incorrectAnswers.Add(decodedAnswer);
                }

                questions.Add(new Question(
                     query,
                     correctAnswer,
                     incorrectAnswers.ToArray()
                 ));

            }
            return questions;
        }
    }
}
