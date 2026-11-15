namespace ITHS_LABB_2_Quiz_Configurator.ViewModels
{
    internal class AnswerOptionViewModel : ViewModelBase
    {
        public string Text { get; }
        public bool IsCorrect { get; }
        private bool _isRevealed;
        public bool IsRevealed
        {
            get => _isRevealed;
            set => SetProperty(ref _isRevealed, value);
        }

        public AnswerOptionViewModel(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

    }
}
