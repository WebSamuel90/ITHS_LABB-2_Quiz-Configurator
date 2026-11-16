using System.Windows;

namespace ITHS_LABB_3_Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportQuestionsDialog.xaml
    /// </summary>
    public partial class ImportQuestionsDialog : Window
    {
        public ImportQuestionsDialog()
        {
            InitializeComponent();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
