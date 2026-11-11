using ITHS_LABB_2_Quiz_Configurator.Enums;
using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Collections.ObjectModel;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack _model;

    public QuestionPackViewModel(QuestionPack model)
    {
        _model = model;
        Questions = new ObservableCollection<Question>(_model.Questions);
    }

    public string Name
    {
        get => _model.Name;
        set
        {
            if (_model.Name == value)
                return;

            _model.Name = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(HasValidName));
        }
    }

    public bool HasValidName => !string.IsNullOrWhiteSpace(Name);

    public Difficulty Difficulty
    {
        get => _model.Difficulty;
        set
        {
            if (_model.Difficulty == value)
                return;

            _model.Difficulty = value;
            NotifyPropertyChanged();
        }
    }
    public Difficulty[] Difficulties { get; } = Enum.GetValues<Difficulty>();

    public int TimeLimitInSeconds
    {
        get => _model.TimeLimitInSeconds;
        set
        {
            if (_model.TimeLimitInSeconds == value)
                return;

            _model.TimeLimitInSeconds = value;
            NotifyPropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; }

    public QuestionPack ToModel()
    {
        return new QuestionPack(Name, Difficulty, TimeLimitInSeconds)
        {
            Questions = Questions.ToList()
        };
    }
}
