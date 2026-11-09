using ITHS_LABB_2_Quiz_Configurator.Dialogs;
using ITHS_LABB_2_Quiz_Configurator.ViewModels;
using System.Windows;

namespace ITHS_LABB_2_Quiz_Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            viewModel.OpenCreateNewPack += (_, draft) =>
            {
                var dialog = new CreateNewPackDialog
                {
                    Owner = this,
                    DataContext = draft
                };

                if (dialog.ShowDialog() == true)
                {
                    viewModel.AddPack(draft);
                }
            };
        }
    }
}