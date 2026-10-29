namespace ITHS_LABB_2_Quiz_Configurator.Models;

internal enum Difficulty { Easy, Medium, Hard }

class QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
{
    public string Name { get; set; } = name;
    public Difficulty Difficulty { get; set; } = difficulty;
    public int TimeLimitInSeconds { get; set; } = timeLimitInSeconds;
    public List<Question> Questions { get; set; } = [];
}
