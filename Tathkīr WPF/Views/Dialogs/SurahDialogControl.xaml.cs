using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.ViewModels.Dialogs;

namespace Tathkīr_WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SurahDialogControl.xaml
    /// </summary>
    public partial class SurahDialogControl : UserControl
    {
        public SurahDialogControl()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                BuildVerseBlock();
                MainBorder.Focus();
            };
        }

        private async void Border_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var border = (Border)sender;

            if (e.Key == Key.Space && !comboBox.IsKeyboardFocusWithin)
            {
                var vm = DataContext as SurahDialogViewModel;
                if (vm == null)
                    return;

                await vm.PlaySurahAsync();
            }
        }

        private void Slider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as SurahDialogViewModel;
            if (vm != null)
                vm.StartSeeking();
        }

        private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as SurahDialogViewModel;
            if (vm != null)
                vm.StopSeeking();
        }

        private void BuildVerseBlock()
        {
            var vm = DataContext as SurahDialogViewModel;
            if (vm?.Surah == null || vm.Surah.Verses == null)
                return;

            var surah = vm.Surah;

            const string bismillah = "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";
            bool haveBismillah = surah.Verses.Count > 0 &&
                                 surah.Verses[0].Text.Text.Contains(bismillah);

            var firstVerse = haveBismillah ? surah.Verses[0].Number.GetIndicDigits() : string.Empty;

            // First verse
            FirstVerseTextBlock.Text = $"{bismillah} {firstVerse}";

            // Remaining verses
            RemainingVersesTextBlock.Inlines.Clear();

            for (int i = haveBismillah ? 1 : 0; i < surah.Verses.Count; i++)
            {
                var verse = surah.Verses[i];
                RemainingVersesTextBlock.Inlines.Add(new Run(verse.Text.Text + " "));
                RemainingVersesTextBlock.Inlines.Add(verse.Number.GetIndicDigits());
                RemainingVersesTextBlock.Inlines.Add(new Run(" "));
            }

        }
    }
}
