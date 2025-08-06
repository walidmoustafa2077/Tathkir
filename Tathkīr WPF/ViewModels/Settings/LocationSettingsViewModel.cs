using Tathkīr_WPF.Helpers;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.ViewModels.Settings
{
    public class LocationSettingsViewModel : ViewModelBase
    {
        public SearchableList<string> AvailableCountries { get; } = new();
        public SearchableList<string> AvailableCities { get; } = new();

        public string? SelectedCountry
        {
            get => AvailableCountries.SelectedItem;
            set => AvailableCountries.SelectedItem = value;
        }

        public string? SelectedCity
        {
            get => AvailableCities.SelectedItem;
            set => AvailableCities.SelectedItem = value;
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
            LoadAsync();

            IsCityEnabled = false;

            AvailableCountries.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AvailableCountries.SelectedItem))
                {
                    if (AvailableCountries.SelectedItem != null)
                    {
                        LoadCitiesForCountry(AvailableCountries.SelectedItem);
                        IsCityEnabled = true;
                    }
                    else
                    {
                        AvailableCities.Items.Clear();
                        IsCityEnabled = false;
                    }
                }
            };
        }

        private async void LoadAsync()
        {
            var countries = await HostService.Instance.GetCountryNamesAsync();
            if (countries != null && countries.Count > 0)
            {
                AvailableCountries.Items.Clear();
                countries.ForEach(country =>
                {
                    if (!string.IsNullOrWhiteSpace(country))
                    {
                        AvailableCountries.Items.Add(country);
                    }
                });

                // set default country to AppSettings
                SelectedCountry = SettingsService.AppSettings.ApiConfig.Country;
                SelectedCity = SettingsService.AppSettings.ApiConfig.City;
            }
            else
            {
                AvailableCountries.Items.Clear();
                SelectedCountry = string.Empty;
                SelectedCity = string.Empty;
            }
        }

        private async void LoadCitiesForCountry(string country)
        {
            var cities = await HostService.Instance.GetCitiesByCountryNameAsync(country);
            if (cities != null && cities.Count > 0)
            {
                AvailableCities.Items.Clear();
                cities.ForEach(city =>
                {
                    if (!string.IsNullOrWhiteSpace(city))
                    {
                        AvailableCities.Items.Add(city);
                    }
                });
            }
            else
            {
                AvailableCities.Items.Clear();
                SelectedCity = string.Empty;
            }
        }

        public void SaveSettings()
        {
            SettingsService.AppSettings.ApiConfig.Country = SelectedCountry ?? string.Empty;
            SettingsService.AppSettings.ApiConfig.City = SelectedCity ?? string.Empty;

            SettingsService.AppSettings.ApiConfig.Address = HostService.Instance.GetCurrentAddress(SelectedCountry, SelectedCity);

            SettingsService.Save(SettingsService.AppSettings);
        }
    }
}
