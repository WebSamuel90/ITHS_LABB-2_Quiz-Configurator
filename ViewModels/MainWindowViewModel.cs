
using ITHS_LABB_2_Quiz_Configurator.Command;
using ITHS_LABB_2_Quiz_Configurator.Enums;
using ITHS_LABB_2_Quiz_Configurator.Models;
using ITHS_LABB_2_Quiz_Configurator.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace ITHS_LABB_2_Quiz_Configurator.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel? _activePack;
        private bool _packsLoaded;
        private string? _loadErrorMessage;
        private string? _saveErrorMessage;
        private ViewModelBase? _currentViewModel;
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizConfigurator", "QuestionPackStorage.json");
        public event EventHandler<QuestionPackViewModel>? OpenCreateNewPack;
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
        public bool HasMoreThanOne => Packs.Count > 1;
        public RelayCommand ShowConfigurationViewModelCommand { get; }
        public RelayCommand ShowPlayerViewModelCommand { get; }
        public RelayCommand OpenCreateNewPackCommand { get; }
        public RelayCommand SelectQuestionPackCommand { get; }
        public RelayCommand DeletePackCommand { get; }

        public QuestionPackViewModel? ActivePack
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

        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        public bool PacksLoaded
        {
            get => _packsLoaded;
            private set => SetProperty(ref _packsLoaded, value);
        }

        public string? LoadErrorMessage
        {
            get => _loadErrorMessage;
            private set => SetProperty(ref _loadErrorMessage, value);
        }

        public string? SaveErrorMessage
        {
            get => _saveErrorMessage;
            private set => SetProperty(ref _saveErrorMessage, value);
        }


        public async Task AddPack(QuestionPackViewModel pack)
        {
            SaveErrorMessage = null;

            var packsToSave = Packs.Select(p => p.ToModel()).ToList();
            packsToSave.Add(pack.ToModel());

            try
            {
                await QuestionPackStorage.SavePacksToJsonFile(packsToSave, _filePath);
                Packs.Add(pack);
                ActivePack = pack;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                SaveErrorMessage = "The question pack could not be saved to the file.";
            }
        }

        public async Task LoadPacks()
        {
            PacksLoaded = false;
            LoadErrorMessage = null;

            try
            {
                var packs = await QuestionPackStorage.LoadPacksFromJsonFile(_filePath);
                Packs.Clear();

                foreach (var pack in packs)
                {
                    Packs.Add(new QuestionPackViewModel(pack));
                }

                if (Packs.Count > 0)
                {
                    ActivePack = Packs[0];
                }

                PacksLoaded = true;
            }
            catch (Exception ex) when (ex is IOException
                           or UnauthorizedAccessException
                           or JsonException)
            {

                LoadErrorMessage = ex switch
                {
                    UnauthorizedAccessException => "The app lacks permission to read the question packs.",
                    JsonException => "The file with the question packs contains invalid JSON or has the wrong format.",
                    _ => "The question packs could not be loaded from the file."
                };
            }
        }

        public async Task DeletePack(QuestionPackViewModel pack)
        {
            if (HasMoreThanOne && Packs.Contains(pack))
            {
                Packs.Remove(pack);

                await UpdatePacks();

                ActivePack = Packs.FirstOrDefault();
            }
        }


        public async Task UpdatePacks()
        {
            SaveErrorMessage = null;

            var packsToSave = Packs.Select(p => p.ToModel()).ToList();

            try
            {
                await QuestionPackStorage.SavePacksToJsonFile(packsToSave, _filePath);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                SaveErrorMessage = "The question pack could not be saved to the file.";
            }
        }

        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }

        public MainWindowViewModel()
        {
            Packs.CollectionChanged += (_, _) => NotifyPropertyChanged(nameof(HasMoreThanOne));

            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            CurrentViewModel = ConfigurationViewModel;

            ShowConfigurationViewModelCommand = new RelayCommand(_ =>
            {
                CurrentViewModel = ConfigurationViewModel;
            });

            ShowPlayerViewModelCommand = new RelayCommand(_ =>
            {
                PlayerViewModel.StartQuiz();
                CurrentViewModel = PlayerViewModel;
            });

            OpenCreateNewPackCommand = new RelayCommand(_ =>
            {
                if (!PacksLoaded)
                {
                    return;
                }

                var draft = new QuestionPackViewModel(
                    new QuestionPack("", Difficulty.Easy));

                OpenCreateNewPack?.Invoke(this, draft);
            });

            SelectQuestionPackCommand = new RelayCommand((param) =>
            {
                if (param is QuestionPackViewModel)
                {
                    ActivePack = (QuestionPackViewModel)param;
                }
            });

            DeletePackCommand = new RelayCommand(async (param) =>
            {
                if (param is QuestionPackViewModel pack)
                {
                    await DeletePack(pack);
                }
            });



            // Temp
            //var questionPack = new QuestionPack("Mina frågor!", Difficulty.Easy);
            //questionPack.Questions.Add(new Question("Vad heter Sveriges huvudstad?", "Stockholm", "Oslo", "Göteborg", "London"));
            //questionPack.Questions.Add(new Question("Vad är 2 + 3", "5", "10", "8", "1"));
            //ActivePack = new QuestionPackViewModel(questionPack);

        }
    }
}
