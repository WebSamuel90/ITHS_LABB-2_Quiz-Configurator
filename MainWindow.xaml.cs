using ITHS_LABB_2_Quiz_Configurator.Dialogs;
using ITHS_LABB_2_Quiz_Configurator.ViewModels;
using System.IO;
using System.Text.Json;
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

            bool packsLoaded = false;

            this.Loaded += async (_, _) =>
            {
                try
                {
                    await viewModel.LoadPacks();
                    packsLoaded = true;
                }
                catch (Exception ex) when (ex is IOException
                           or UnauthorizedAccessException
                           or JsonException)
                {
                    string message = ex switch
                    {
                        UnauthorizedAccessException => "The app lacks permission to read the question packs.",
                        JsonException => "The file with the question packs contains invalid JSON or has the wrong format.",
                        _ => "The question packs could not be loaded from the file."
                    };

                    MessageBox.Show(
                        this,
                        message,
                        "Read Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            };


            viewModel.OpenCreateNewPack += async (_, draft) =>
            {
                if (!packsLoaded)
                {
                    return;
                }

                var dialog = new CreateNewPackDialog
                {
                    Owner = this,
                    DataContext = draft
                };

                if (dialog.ShowDialog() != true)
                {
                    return;
                }

                try
                {
                    await viewModel.AddPack(draft);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {

                    MessageBox.Show(
                        this,
                        "The question pack could not be saved to the file.",
                        "Save Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

            };
        }
    }
}