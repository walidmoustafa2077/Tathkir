using System.Collections.ObjectModel;
using System.IO;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services;
using Tathkīr_WPF.ViewModels.Dialogs;
using Tathkīr_WPF.Views.Dialogs;


namespace Tathkīr_WPF.ViewModels.Generic
{
    public class AthkarViewModel : ViewModelBase
    {
        public ObservableCollection<AthkarItem> AthkarItems { get; set; }
        private AthkarItem? _selectedAthkarItem = null;
        public AthkarItem? SelectedAthkarItem
        {
            get => _selectedAthkarItem;
            set
            {
                _selectedAthkarItem = value;
                OnPropertyChanged();

                HandleDialog();
            }
        }

        private void HandleDialog()
        {
            var vm = new ThikrDialogViewModel
            {
                Title = Strings.Morning_Thikr,
                Items = new ObservableCollection<ThikrDialogItem>
                    {
                        new ThikrDialogItem { Title = "أَعُوذُ بِاللهِ مِنْ الشَّيْطَانِ الرَّجِيمِ",
                            Content = "اللّهُ لاَ إِلَـهَ إِلاَّ هُوَ الْحَيُّ الْقَيُّومُ لاَ تَأْخُذُهُ سِنَةٌ وَلاَ نَوْمٌ لَّهُ مَا فِي السَّمَاوَاتِ وَمَا فِي الأَرْضِ مَن ذَا الَّذِي يَشْفَعُ عِنْدَهُ إِلاَّ بِإِذْنِهِ يَعْلَمُ مَا بَيْنَ أَيْدِيهِمْ وَمَا خَلْفَهُمْ وَلاَ يُحِيطُونَ بِشَيْءٍ مِّنْ عِلْمِهِ إِلاَّ بِمَا شَاء وَسِعَ كُرْسِيُّهُ السَّمَاوَاتِ وَالأَرْضَ وَلاَ يَؤُودُهُ حِفْظُهُمَا وَهُوَ الْعَلِيُّ الْعَظِيمُ. [آية الكرسى - البقرة 255]. ",
                            Description = "من قالها حين يصبح أجير من الجن حتى يمسى ومن قالها حين يمسى أجير من الجن حتى يصبح. ",
                            Count = 100 },

                            new ThikrDialogItem { Title = "بِسْمِ اللهِ الرَّحْمنِ الرَّحِيم",
                            Content = "قُلْ هُوَ ٱللَّهُ أَحَدٌ، ٱللَّهُ ٱلصَّمَدُ، لَمْ يَلِدْ وَلَمْ يُولَدْ، وَلَمْ يَكُن لَّهُۥ كُفُوًا أَحَدٌۢ.",
                            Description = "من قالها حين يصبح وحين يمسى كفته من كل شىء (الإخلاص والمعوذتين). ",
                            Count = 3 },

                            new ThikrDialogItem { Title = "بِسْمِ اللهِ الرَّحْمنِ الرَّحِيم",
                            Content = "قُلْ هُوَ ٱللَّهُ أَحَدٌ، ٱللَّهُ ٱلصَّمَدُ، لَمْ يَلِدْ وَلَمْ يُولَدْ، وَلَمْ يَكُن لَّهُۥ كُفُوًا أَحَدٌۢ.",
                            Description = "من قالها حين يصبح وحين يمسى كفته من كل شىء (الإخلاص والمعوذتين). ",
                            Count = 3 },

                            new ThikrDialogItem { Title = "بِسْمِ اللهِ الرَّحْمنِ الرَّحِيم",
                            Content = "قُلْ أَعُوذُ بِرَبِّ ٱلْفَلَقِ، مِن شَرِّ مَا خَلَقَ، وَمِن شَرِّ غَاسِقٍ إِذَا وَقَبَ، وَمِن شَرِّ ٱلنَّفَّٰثَٰتِ فِى ٱلْعُقَدِ، وَمِن شَرِّ حَاسِدٍ إِذَا حَسَدَ. ",
                            Description = "من قالها حين يصبح وحين يمسى كفته من كل شىء (الإخلاص والمعوذتين). ",
                            Count = 3 },

                            new ThikrDialogItem { Title = "بِسْمِ اللهِ الرَّحْمنِ الرَّحِيم",
                            Content = "قُلْ أَعُوذُ بِرَبِّ ٱلنَّاسِ، مَلِكِ ٱلنَّاسِ، إِلَٰهِ ٱلنَّاسِ، مِن شَرِّ ٱلْوَسْوَاسِ ٱلْخَنَّاسِ، ٱلَّذِى يُوَسْوِسُ فِى صُدُورِ ٱلنَّاسِ، مِنَ ٱلْجِنَّةِ وَٱلنَّاسِ. ",
                            Description = "من قالها حين يصبح وحين يمسى كفته من كل شىء (الإخلاص والمعوذتين). ",
                            Count = 3 },

                            new ThikrDialogItem {
                            Content =  "أَصْـبَحْنا وَأَصْـبَحَ المُـلْكُ لله وَالحَمدُ لله ، لا إلهَ إلاّ اللّهُ وَحدَهُ لا شَريكَ لهُ، لهُ المُـلكُ ولهُ الحَمْـد، وهُوَ على كلّ شَيءٍ قدير ، رَبِّ أسْـأَلُـكَ خَـيرَ ما في هـذا اليوم وَخَـيرَ ما بَعْـدَه ، وَأَعـوذُ بِكَ مِنْ شَـرِّ ما في هـذا اليوم وَشَرِّ ما بَعْـدَه، رَبِّ أَعـوذُبِكَ مِنَ الْكَسَـلِ وَسـوءِ الْكِـبَر ، رَبِّ أَعـوذُ بِكَ مِنْ عَـذابٍ في النّـارِ وَعَـذابٍ في القَـبْر. ",
                            Count = 1 },

                            new ThikrDialogItem {
                            Content =  "اللّهـمَّ أَنْتَ رَبِّـي لا إلهَ إلاّ أَنْتَ ، خَلَقْتَنـي وَأَنا عَبْـدُك ، وَأَنا عَلـى عَهْـدِكَ وَوَعْـدِكَ ما اسْتَـطَعْـت ، أَعـوذُبِكَ مِنْ شَـرِّ ما صَنَـعْت ، أَبـوءُ لَـكَ بِنِعْـمَتِـكَ عَلَـيَّ وَأَبـوءُ بِذَنْـبي فَاغْفـِرْ لي فَإِنَّـهُ لا يَغْـفِرُ الذُّنـوبَ إِلاّ أَنْتَ . ",
                            Description = "من قالها موقنا بها حين يمسى ومات من ليلته دخل الجنة وكذلك حين يصبح.",
                            Count = 1 },
                    }
            };
            var dialog = new ThikrDialogControl { DataContext = vm };
            DialogService.Instance.ShowDialogGeneric(vm);
        }

        public AthkarViewModel()
        {
            var iconsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Icons");

            AthkarItems = new ObservableCollection<AthkarItem>
            {
                new AthkarItem { Name = Strings.Morning_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Evening_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Athan_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Prayer_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.After_Prayer_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Mosque_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Bedtime_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Wake_up_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Eating_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png") },
                new AthkarItem { Name = Strings.Entering_Leaving_Bathroom_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
                new AthkarItem { Name = Strings.Wudu_Thikr, Icon = Path.Combine(iconsPath, "mosque_10010709.png")},
            };
        }
    }

}
