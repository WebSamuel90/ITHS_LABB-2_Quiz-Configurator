

using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Collections.ObjectModel;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; }

        private QuestionPackViewModel _activePack;
        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set => SetProperty(ref _activePack, value);
        }

        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            // Temp
            var questionPack = new Models.QuestionPack("Mina frågor!", Difficulty.Easy);
            questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Oslo", "Göteborg", "London"));
            questionPack.Questions.Add(new Question("Vad är 2 + 3", "5", "10", "8", "1"));
            ActivePack = new QuestionPackViewModel(new QuestionPack("Mina frågor!"));
        }
    }
}
