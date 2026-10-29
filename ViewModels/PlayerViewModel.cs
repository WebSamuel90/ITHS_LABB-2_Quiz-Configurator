namespace ITHS_LABB_2_Quiz_Configurator.ViewModels;

class PlayerViewModel
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
}
