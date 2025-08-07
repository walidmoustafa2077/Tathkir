using System.IO;
using System.Text.Json;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.Helpers
{
    public static class DynamicStrings
    {
        public static Dictionary<string, LocalizedStringPair> Strings { get; set; } = new();

        private static string _saveFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "strings.json");

        public static string AddString(string key, string value, string localizedValue)
        {
            if (!Strings.ContainsKey(key))
            {
                Strings[key] = new LocalizedStringPair { Value = value, LocalizedValue = localizedValue };
                SaveStrings();
            }
            return value;
        }

        public static string UpdateString(string key, string value, string localizedValue)
        {
            if (Strings.ContainsKey(key))
            {
                Strings[key] = new LocalizedStringPair { Value = value, LocalizedValue = localizedValue };
                SaveStrings();
            }
            else
            {
                AddString(key, value, localizedValue);
            }
            return value;
        }


        public static string GetString(string key, bool localized = false)
        {
            if (Strings.TryGetValue(key, out var values))
            {
                return localized ? values.LocalizedValue : values.Value;
            }
            return string.Empty;
        }

        public static void LoadStrings()
        {
            try
            {
                if (File.Exists(_saveFilePath))
                {
                    var json = File.ReadAllText(_saveFilePath);
                    Strings = JsonSerializer.Deserialize<Dictionary<string, LocalizedStringPair>>(json)
                              ?? new Dictionary<string, LocalizedStringPair>();
                }
            }
            catch (Exception ex)
            {
                DialogService.Instance.ShowError("Error loading strings", ex.Message);
            }
        }

        private static void SaveStrings()
        {
            try
            {
                var json = JsonSerializer.Serialize(Strings, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                File.WriteAllText(_saveFilePath, json);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                DialogService.Instance.ShowError("Error saving strings", ex.Message);
            }
        }

        public class LocalizedStringPair
        {
            public string Value { get; set; } = string.Empty;
            public string LocalizedValue { get; set; } = string.Empty;
        }

    }
}
