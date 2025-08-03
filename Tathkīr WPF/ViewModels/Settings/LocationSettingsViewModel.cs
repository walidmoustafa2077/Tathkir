using System.Collections.ObjectModel;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class LocationSettingsViewModel : ViewModelBase
    {
        public ObservableCollection<string> AvailableCountries { get; set; }
        public ObservableCollection<string> AvailableCities { get; set; }

        private string _selectedCountry = string.Empty;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                {
                    LoadCitiesForCountry(_selectedCountry);
                    IsCityEnabled = !string.IsNullOrEmpty(_selectedCountry);
                }
            }
        }

        private string _selectedCity = string.Empty;
        public string SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
        }

        private bool _isCityEnabled;
        public bool IsCityEnabled
        {
            get => _isCityEnabled;
            set => SetProperty(ref _isCityEnabled, value);
        }

        public LocationSettingsViewModel()
        {
            // Sample data for testing
            AvailableCountries = new ObservableCollection<string>
            {
                "Egypt", "Saudi Arabia", "UAE", "Morocco", "Indonesia"
            };

            AvailableCities = new ObservableCollection<string>();

            IsCityEnabled = false;
        }

        private void LoadCitiesForCountry(string country)
        {
            AvailableCities.Clear();

            if (string.IsNullOrWhiteSpace(country)) return;

            // Sample test data
            switch (country)
            {
                case "Egypt":
                    AvailableCities.Add("Cairo");
                    AvailableCities.Add("Alexandria");
                    AvailableCities.Add("Giza");
                    AvailableCities.Add("Shubra El Kheima");
                    AvailableCities.Add("Port Said");
                    AvailableCities.Add("Suez");
                    AvailableCities.Add("Luxor");
                    AvailableCities.Add("Aswan");
                    break;
                case "Saudi Arabia":
                    AvailableCities.Add("Riyadh");
                    AvailableCities.Add("Jeddah");
                    AvailableCities.Add("Makkah");
                    break;
                case "UAE":
                    AvailableCities.Add("Dubai");
                    AvailableCities.Add("Abu Dhabi");
                    AvailableCities.Add("Sharjah");
                    break;
                case "Morocco":
                    AvailableCities.Add("Casablanca");
                    AvailableCities.Add("Marrakech");
                    AvailableCities.Add("Rabat");
                    break;
                case "Indonesia":
                    AvailableCities.Add("Jakarta");
                    AvailableCities.Add("Bandung");
                    AvailableCities.Add("Surabaya");
                    break;
            }
        }
    }
}
