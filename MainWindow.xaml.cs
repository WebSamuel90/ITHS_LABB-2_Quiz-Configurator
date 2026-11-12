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

            bool savingOnClose = false;
            bool savedOnClose = false;

            Closing += async (_, e) =>
            {
                if (!viewModel.PacksLoaded || savedOnClose)
                    return;

                e.Cancel = true;

                if (savingOnClose)
                    return;

                savingOnClose = true;

                try
                {
                    await viewModel.UpdatePacks();

                    var errorMessage = viewModel.SaveErrorMessage;

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        MessageBox.Show(
                            this,
                            errorMessage,
                            "Save Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        return;
                    }
                }
                finally
                {
                    savingOnClose = false;
                }

                savedOnClose = true;
                Close();
            };

            this.Loaded += async (_, _) =>
            {
                await viewModel.LoadPacks();
                var errorMessage = viewModel.LoadErrorMessage;

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(
                        this,
                        errorMessage,
                        "Read Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            };


            viewModel.OpenCreateNewPack += async (_, draft) =>
            {
                var dialog = new CreateNewPackDialog
                {
                    Owner = this,
                    DataContext = draft
                };

                if (dialog.ShowDialog() != true)
                {
                    return;
                }

                await viewModel.AddPack(draft);
                var errorMessage = viewModel.SaveErrorMessage;

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(
                        this,
                        errorMessage,
                        "Save Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            };
        }
    }
}