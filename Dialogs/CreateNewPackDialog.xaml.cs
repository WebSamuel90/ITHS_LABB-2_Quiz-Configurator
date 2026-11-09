using System.Windows;

namespace ITHS_LABB_2_Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                packName.Focus();
                packName.SelectAll();
            };
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
