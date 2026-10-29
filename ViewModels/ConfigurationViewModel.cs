namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class ConfigurationViewModel
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
}
