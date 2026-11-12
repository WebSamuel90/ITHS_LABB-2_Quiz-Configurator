using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Diagnostics;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public List<Question> QuizQuestions { get; } = [];
    private Question? _currentQuestion;
    private int _currentQuestionIndex;
    private int _score;
    private bool? _isAnswerCorrect;
    private string? _selectedAnswer;
    private bool _IsQuizFinished;
    private List<string> _currentAnswers = [];
    public RelayCommand SubmitAnswerCommand { get; }
    public RelayCommand RestartQuizCommand { get; }

    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set => SetProperty(ref _currentQuestion, value);
    }

    public List<string> CurrentAnswers
    {
        get => _currentAnswers;
        set
        {
            SetProperty(ref _currentAnswers, value);
            NotifyPropertyChanged(nameof(CorrectAnswerIndex));
        }
    }

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        set
        {
            if (SetProperty(ref _currentQuestionIndex, value))
            {
                NotifyPropertyChanged(nameof(CurrentQuestionNumber));
            }
        }
    }

    public int Score
    {
        get => _score;
        set => SetProperty(ref _score, value);
    }
    public bool? IsAnswerCorrect
    {
        get => _isAnswerCorrect;
        set => SetProperty(ref _isAnswerCorrect, value);
    }

    public string? SelectedAnswer
    {
        get => _selectedAnswer;
        set
        {
            if (SetProperty(ref _selectedAnswer, value))
            {
                NotifyPropertyChanged(nameof(SelectedAnswerIndex));
            }
        }
    }

    public bool IsQuizFinished
    {
        get => _IsQuizFinished;
        set => SetProperty(ref _IsQuizFinished, value);
    }

    public int SelectedAnswerIndex =>
        SelectedAnswer == null
            ? -1
            : CurrentAnswers.IndexOf(SelectedAnswer);

    public int CorrectAnswerIndex =>
     CurrentQuestion == null
         ? -1
         : CurrentAnswers.IndexOf(CurrentQuestion.CorrectAnswer);

    public int CurrentQuestionNumber => CurrentQuestionIndex + 1;

    public void StartQuiz()
    {
        QuizQuestions.Clear();
        CurrentQuestionIndex = 0;
        CurrentAnswers = [];
        Score = 0;
        IsAnswerCorrect = null;
        SelectedAnswer = null;
        IsQuizFinished = false;

        var activePack = _mainWindowViewModel?.ActivePack;

        if (activePack != null)
        {
            QuizQuestions.AddRange(activePack.Questions);
        }

        ShuffleIndex(QuizQuestions);
        CurrentQuestion = QuizQuestions.Count > 0 ? QuizQuestions[CurrentQuestionIndex] : null;

        if (CurrentQuestion != null)
        {
            UpdateCurrentAnswers(CurrentQuestion);
        }
    }

    public async Task NextQuestion()
    {
        await Task.Delay(1500);

        if (CurrentQuestionIndex < (QuizQuestions.Count - 1))
        {
            SelectedAnswer = null;
            CurrentQuestionIndex++;
            CurrentQuestion = QuizQuestions[CurrentQuestionIndex];
            UpdateCurrentAnswers(CurrentQuestion);
            IsAnswerCorrect = null;
        }
        else
        {
            Debug.WriteLine("Quiz finished");
            IsQuizFinished = true;
        }

    }

    public void UpdateCurrentAnswers(Question CurrentQuestion)
    {
        var answers = new List<string>();
        answers.AddRange(CurrentQuestion.IncorrectAnswers);
        answers.Add(CurrentQuestion.CorrectAnswer);

        ShuffleIndex(answers);
        CurrentAnswers = answers;
    }

    public static void ShuffleIndex<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        SubmitAnswerCommand = new RelayCommand(async (param) =>
        {
            if (param is string submittedAnswer && CurrentQuestion != null && IsAnswerCorrect == null)
            {
                SelectedAnswer = submittedAnswer;
                if (submittedAnswer == CurrentQuestion.CorrectAnswer)
                {
                    Score++;
                    IsAnswerCorrect = true;
                    Debug.WriteLine("Correct!");
                    Debug.WriteLine(Score);
                }
                else
                {
                    IsAnswerCorrect = false;
                    Debug.WriteLine("Incorrect!");
                    Debug.WriteLine(Score);

                }

                await NextQuestion();
            }
        });

        RestartQuizCommand = new RelayCommand(_ =>
        {
            StartQuiz();
        });
    }
}
