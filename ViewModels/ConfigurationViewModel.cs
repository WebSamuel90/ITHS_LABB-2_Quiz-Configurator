using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Windows;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public event EventHandler? OpenPackSettings;
    public RelayCommand OpenPackSettingsCommand { get; }
    public RelayCommand CreateNewQuestionCommand { get; }
    public RelayCommand DeleteQuestionCommand { get; }

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    public void NotifyActivePackChanged() => NotifyPropertyChanged(nameof(ActivePack));

    private Question? _selectedQuestion;
    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            if (SetProperty(ref _selectedQuestion, value))
                NotifyPropertyChanged(nameof(HasSelectedQuestion));
        }
    }

    public Visibility HasSelectedQuestion => SelectedQuestion != null ? Visibility.Visible : Visibility.Hidden;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {

        CreateNewQuestionCommand = new RelayCommand(_ =>
        {
            var newQuestion = new Question("New Question", "", ["", "", ""]);
            ActivePack?.Questions.Add(newQuestion);
        });

        DeleteQuestionCommand = new RelayCommand(_ =>
        {
            if (SelectedQuestion != null && ActivePack != null)
            {
                ActivePack.Questions.Remove(SelectedQuestion);
                SelectedQuestion = null;
            }
        });


        _mainWindowViewModel = mainWindowViewModel;
        OpenPackSettingsCommand = new RelayCommand(_ => OpenPackSettings?.Invoke(this, EventArgs.Empty));
    }
}
