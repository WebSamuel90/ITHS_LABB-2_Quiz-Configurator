namespace ITHS_LABB_2_Quiz_Configurator.ViewModels
{
    internal class AnswerOptionViewModel : ViewModelBase
    {
        public string Text { get; }
        public bool IsCorrect { get; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public AnswerOptionViewModel(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

    }
}
