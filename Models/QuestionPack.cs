using ITHS_LABB_2_Quiz_Configurator.Enums;
namespace ITHS_LABB_2_Quiz_Configurator.Models;

class QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
{
    public string Name { get; set; } = name;
    public Difficulty Difficulty { get; set; } = difficulty;
    public int TimeLimitInSeconds { get; set; } = timeLimitInSeconds;
    public List<Question> Questions { get; set; } = [];
}
