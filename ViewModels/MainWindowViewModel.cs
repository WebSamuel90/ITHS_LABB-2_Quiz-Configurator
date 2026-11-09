
using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Enums;
using ITHS_LABB_2_Quiz_Configurator.Models;
using System.Collections.ObjectModel;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel _activePack;
        public event EventHandler<QuestionPackViewModel>? OpenCreateNewPack;
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        public RelayCommand OpenCreateNewPackCommand { get; }

        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set
            {
                if (SetProperty(ref _activePack, value))
                {
                    ConfigurationViewModel?.NotifyActivePackChanged();
                }
            }
        }

        public void AddPack(QuestionPackViewModel pack)
        {
            Packs.Add(pack);
            ActivePack = pack;
        }

        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            OpenCreateNewPackCommand = new RelayCommand(_ =>
            {
                var draft = new QuestionPackViewModel(
                    new QuestionPack("", Difficulty.Easy));

                OpenCreateNewPack?.Invoke(this, draft);
            });

            // Temp
            var questionPack = new QuestionPack("Mina frågor!", Difficulty.Easy);
            questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Oslo", "Göteborg", "London"));
            questionPack.Questions.Add(new Question("Vad är 2 + 3", "5", "10", "8", "1"));
            ActivePack = new QuestionPackViewModel(questionPack);

        }
    }
}
