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
                        UnauthorizedAccessException => "Appen saknar behörighet att läsa frågepaketen.",
                        JsonException => "Filen med frågepaketen innehåller ogiltig JSON eller har fel format.",
                        _ => "Frågepaketen kunde inte läsas från filen."
                    };

                    MessageBox.Show(
                        this,
                        message,
                        "Läsfel",
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
                        "Frågepaketet kunde inte sparas till filen.",
                        "Sparfel",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

            };
        }
    }
}