namespace ITHS_LABB_2_Quiz_Configurator.Models;

class Question(string query, string correctAnswer, string[] incorrectAnswers)
{
    public string Query { get; set; } = query;
    public string CorrectAnswer { get; set; } = correctAnswer;
    public string[] IncorrectAnswers { get; set; } = incorrectAnswers;
}
