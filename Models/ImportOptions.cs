using ITHS_LABB_3_Quiz_Configurator.Enums;

namespace ITHS_LABB_3_Quiz_Configurator.Models
{

    class ImportOptions(int categoryId = 9, Difficulty selectedDifficulty = Difficulty.Medium, int questionCount = 1)
    {
        public int CategoryId { get; set; } = categoryId;
        public Difficulty SelectedDifficulty { get; set; } = selectedDifficulty;
        public int QuestionCount { get; set; } = questionCount;
        public Difficulty[] Difficulties { get; } = Enum.GetValues<Difficulty>();

        public Dictionary<int, string> Categories { get; } = new()
        {
            { 9, "General Knowledge" },
            { 10, "Entertainment: Books" },
            { 11, "Entertainment: Film" },
            { 12, "Entertainment: Music" },
            { 15, "Entertainment: Video Games" },
            { 17, "Science & Nature" },
            { 18, "Science: Computers" },
            { 21, "Sports" },
            { 22, "Geography" },
            { 23, "History" }
        };
    }
}
