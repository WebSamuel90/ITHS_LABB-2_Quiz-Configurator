using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Models;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public event EventHandler? OpenPackSettings;
    public RelayCommand OpenPackSettingsCommand { get; }

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    public void NotifyActivePackChanged() => NotifyPropertyChanged(nameof(ActivePack));

    private Question? _selectedQuestion;
    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set => SetProperty(ref _selectedQuestion, value);
    }

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        OpenPackSettingsCommand = new RelayCommand(_ => OpenPackSettings?.Invoke(this, EventArgs.Empty));
    }
}
