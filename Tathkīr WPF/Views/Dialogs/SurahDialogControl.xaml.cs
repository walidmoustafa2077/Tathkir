using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Tathkīr_WPF.ViewModels.Dialogs;

namespace Tathkīr_WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SurahDialogControl.xaml
    /// </summary>
    public partial class SurahDialogControl : UserControl
    {
        private bool _onFirstLoad = false;
        public SurahDialogControl()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                BuildVerseBlock();
            };
        }

        private async void Border_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var border = (Border)sender;

            if (e.Key == Key.Enter)
            {
                var match = comboBox.Items
                   .Cast<object>()
                   .FirstOrDefault(i =>
                       NormalizeArabic(i?.ToString() ?? string.Empty)
                           .Contains(NormalizeArabic(comboBox.Text ?? string.Empty),
                                     StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    comboBox.SelectedItem = match;
                    comboBox.IsDropDownOpen = false;

                    // Move focus away from ComboBox
                    border.Focus();
                }
            }
            else if (e.Key == Key.Space && !comboBox.IsKeyboardFocusWithin)
            {
                var vm = DataContext as SurahDialogViewModel;
                if (vm == null || vm.ReadersView == null)
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


        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_onFirstLoad)
            {
                _onFirstLoad = true;
                return;
            }

            var vm = DataContext as SurahDialogViewModel;
            if (vm == null || vm.ReadersView == null)
                return;

            if (comboBox.Text != vm.SearchText)
            {
                vm.SearchText = comboBox.Text;
            }
            comboBox.IsDropDownOpen = true; // Keep dropdown open while typing
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

            var firstVerse = haveBismillah ? GetIndicDigits(surah.Verses[0].Number) : string.Empty;

            // First verse
            FirstVerseTextBlock.Text = $"{bismillah} {firstVerse}";

            // Remaining verses
            RemainingVersesTextBlock.Inlines.Clear();

            for (int i = haveBismillah ? 1 : 0; i < surah.Verses.Count; i++)
            {
                var verse = surah.Verses[i];
                RemainingVersesTextBlock.Inlines.Add(new Run(verse.Text.Text + " "));
                RemainingVersesTextBlock.Inlines.Add(GetIndicDigits(verse.Number));
                RemainingVersesTextBlock.Inlines.Add(new Run(" "));
            }

        }

        private string GetIndicDigits(int number)
        {
            var indicDigits = new[] { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
            return string.Concat(number.ToString().Select(c => indicDigits[c - '0']));
        }

        private string NormalizeArabic(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove Tatweel (ـ)
            input = input.Replace("ـ", "");

            // Normalize into FormD (decomposed characters)
            string formD = input.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (var ch in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                // Skip diacritics (NonSpacingMark)
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            // Return recomposed text
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
