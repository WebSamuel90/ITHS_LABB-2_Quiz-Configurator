using ITHS_LABB_2_Quiz_Configurator.Dialogs;
using ITHS_LABB_2_Quiz_Configurator.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ITHS_LABB_2_Quiz_Configurator.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        private ConfigurationViewModel? _configurationViewModel;
        public ConfigurationView()
        {
            InitializeComponent();

            DataContextChanged += (_, e) =>
            {
                if (_configurationViewModel != null)
                {
                    _configurationViewModel.OpenPackSettings -=
                        ConfigurationViewModel_OpenPackSettings;
                }

                _configurationViewModel = e.NewValue as ConfigurationViewModel;

                if (_configurationViewModel != null)
                {
                    _configurationViewModel.OpenPackSettings +=
                        ConfigurationViewModel_OpenPackSettings;
                }
            };
        }

        private void ConfigurationViewModel_OpenPackSettings(object? sender, EventArgs e)
        {
            var dialog = new PackOptionsDialog()
            {
                DataContext = _configurationViewModel?.ActivePack,
                Owner = Window.GetWindow(this),
            };

            if (dialog.ShowDialog() == true) { }
        }
    }
}
