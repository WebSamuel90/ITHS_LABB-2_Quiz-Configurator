using ITHS_LABB_3_Quiz_Configurator.Command;
using ITHS_LABB_3_Quiz_Configurator.Models;
using System.Diagnostics;
using System.Windows.Threading;

namespace ITHS_LABB_3_Quiz_Configurator.ViewModels;

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
    private readonly DispatcherTimer _questionTimer;
    private int _questionTimeLimit;
    private TimeSpan _timeRemaining;
    private Stopwatch _stopwatch = new();
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


    public TimeSpan TimeRemaining
    {
        get => _timeRemaining;
        private set
        {
            SetProperty(ref _timeRemaining, value);
            NotifyPropertyChanged(nameof(SecondsRemaining));
        }
    }
    public int SecondsRemaining => (int)Math.Ceiling(TimeRemaining.TotalSeconds);

    public async void QuestionTimer_Tick(object? sender, EventArgs e)
    {
        TimeSpan remaining = TimeSpan.FromSeconds(_questionTimeLimit) - _stopwatch.Elapsed;

        if (remaining > TimeSpan.Zero)
        {
            TimeRemaining = remaining;
        }
        else
        {
            await HandleTimeOut();
        }
    }

    private void StartQuestionTimer()
    {
        TimeRemaining = TimeSpan.FromSeconds(_questionTimeLimit);
        _stopwatch.Restart();
        _questionTimer.Start();
    }

    private void StopQuestionTimer()
    {
        _stopwatch.Stop();
        _questionTimer.Stop();
    }

    private async Task HandleTimeOut()
    {
        if (IsAnswerCorrect != null)
        { return; }

        StopQuestionTimer();
        TimeRemaining = TimeSpan.Zero;
        IsAnswerCorrect = false;
        var correctAnswer = CurrentAnswers.FirstOrDefault(answer => answer.IsCorrect);
        correctAnswer?.IsRevealed = true;

        await NextQuestion();
    }

    public int CurrentQuestionNumber => CurrentQuestionIndex + 1;


    public void StartQuiz()
    {
        _nextQuestionCts?.Cancel();
        _nextQuestionCts?.Dispose();
        _nextQuestionCts = null;

        StopQuestionTimer();

        QuizQuestions.Clear();
        CurrentQuestionIndex = 0;
        CurrentAnswers = [];
        Score = 0;
        IsAnswerCorrect = null;
        IsQuizFinished = false;

        var activePack = _mainWindowViewModel?.ActivePack;
        _questionTimeLimit = activePack?.TimeLimitInSeconds ?? 30;

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
            StartQuestionTimer();
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
            StartQuestionTimer();
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

        _questionTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        _questionTimer.Tick += QuestionTimer_Tick;


        SubmitAnswerCommand = new RelayCommand(async (param) =>
        {
            if (param is AnswerOptionViewModel answer && CurrentQuestion != null && IsAnswerCorrect == null && CurrentAnswers.Contains(answer))
            {
                if (_stopwatch.Elapsed >= TimeSpan.FromSeconds(_questionTimeLimit))
                {
                    await HandleTimeOut();
                    return;
                }

                StopQuestionTimer();

                if (answer.IsCorrect)
                {
                    Score++;
                    IsAnswerCorrect = true;
                }
                else
                {
                    IsAnswerCorrect = false;
                }

                var correctAnswer = CurrentAnswers.FirstOrDefault(a => a.IsCorrect);
                if (correctAnswer != null)
                {
                    correctAnswer.IsRevealed = true;
                }

                answer.IsRevealed = true;

                await NextQuestion();
            }
        });

        RestartQuizCommand = new RelayCommand(_ =>
        {
            StartQuiz();
        });
    }
}
