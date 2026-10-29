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

            DataContext = new MainWindowViewModel();
        }
    }
}