using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Models;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public List<Question> QuizQuestions { get; } = [];
    private Question? _currentQuestion;
    private int _currentQuestionIndex;
    private int _score;
    private bool? _isAnswerCorrect;
    private bool _isQuizFinished;
    private CancellationTokenSource? _nextQuestionCts;
    private List<AnswerOptionViewModel> _currentAnswers = [];
    public RelayCommand SubmitAnswerCommand { get; }
    public RelayCommand RestartQuizCommand { get; }

    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set => SetProperty(ref _currentQuestion, value);
    }

    public List<AnswerOptionViewModel> CurrentAnswers
    {
        get => _currentAnswers;
        set => SetProperty(ref _currentAnswers, value);
    }

    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        private set
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
        private set => SetProperty(ref _score, value);
    }
    public bool? IsAnswerCorrect
    {
        get => _isAnswerCorrect;
        set => SetProperty(ref _isAnswerCorrect, value);
    }

    public bool IsQuizFinished
    {
        get => _isQuizFinished;
        private set => SetProperty(ref _isQuizFinished, value);
    }

    public int CurrentQuestionNumber => CurrentQuestionIndex + 1;

    public void StartQuiz()
    {
        _nextQuestionCts?.Cancel();
        _nextQuestionCts?.Dispose();
        _nextQuestionCts = null;

        QuizQuestions.Clear();
        CurrentQuestionIndex = 0;
        CurrentAnswers = [];
        Score = 0;
        IsAnswerCorrect = null;
        IsQuizFinished = false;

        var activePack = _mainWindowViewModel?.ActivePack;

        if (activePack != null)
        {
            QuizQuestions.AddRange(activePack.Questions);
        }

        if (QuizQuestions.Count == 0)
        {
            CurrentQuestion = null;
            CurrentAnswers = [];
            IsQuizFinished = true;
            return;
        }

        Shuffle(QuizQuestions);
        CurrentQuestion = QuizQuestions.Count > 0 ? QuizQuestions[CurrentQuestionIndex] : null;

        if (CurrentQuestion != null)
        {
            UpdateCurrentAnswers(CurrentQuestion);
        }
    }

    public async Task NextQuestion()
    {
        var cts = new CancellationTokenSource();
        _nextQuestionCts = cts;

        try
        {
            await Task.Delay(1500, cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        finally
        {
            if (ReferenceEquals(_nextQuestionCts, cts))
            {
                _nextQuestionCts = null;
                cts.Dispose();
            }
        }

        if (CurrentQuestionIndex < (QuizQuestions.Count - 1))
        {
            CurrentQuestionIndex++;
            CurrentQuestion = QuizQuestions[CurrentQuestionIndex];
            UpdateCurrentAnswers(CurrentQuestion);
            IsAnswerCorrect = null;
        }
        else
        {
            IsQuizFinished = true;
        }

    }

    private void UpdateCurrentAnswers(Question question)
    {
        var answers = new List<AnswerOptionViewModel>
        {
            new(question.CorrectAnswer, true)
        };

        answers.AddRange(
            question.IncorrectAnswers.Select(answer => new AnswerOptionViewModel(answer, false))
        );

        Shuffle(answers);
        CurrentAnswers = answers;
    }

    public static void Shuffle<T>(List<T> list)
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
            if (param is AnswerOptionViewModel answer && CurrentQuestion != null && IsAnswerCorrect == null && CurrentAnswers.Contains(answer))
            {
                if (answer.IsCorrect)
                {
                    Score++;
                    IsAnswerCorrect = true;
                }
                else
                {
                    IsAnswerCorrect = false;
                }

                answer.IsSelected = true;

                await NextQuestion();
            }
        });

        RestartQuizCommand = new RelayCommand(_ =>
        {
            StartQuiz();
        });
    }
}
