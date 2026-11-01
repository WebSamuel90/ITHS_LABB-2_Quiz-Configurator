using ITHS_LABB_2_Quiz_Configurator.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace ITHS_LABB_2_Quiz_Configurator.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : UserControl
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PackOptionsDialog
            {
                DataContext = DataContext,
                Owner = Window.GetWindow(this)
            };

            dialog.ShowDialog();
        }
    }
}
