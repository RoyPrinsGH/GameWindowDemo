using Newtonsoft.Json;

namespace WinFormsApp1
{
    public sealed class WindowSettings
    {
        public bool ConsoleEnabled { get; set; }
    }

    internal static class Settings
    {
        private const string _settingsPath = "settings.json";
        public static WindowSettings Window => GetSettingsJsonObject().WindowSettings;

        private sealed class SettingsJsonObject
        {
            public WindowSettings WindowSettings = new() { ConsoleEnabled = false };
        }

        private static SettingsJsonObject? _settingsObject = null;
        private static SettingsJsonObject GetSettingsJsonObject()
        {
            if (_settingsObject is not null)
            {
                return _settingsObject;
            }

            string settingsJSONContent = File.ReadAllText(_settingsPath);
            SettingsJsonObject? convertedSettings = JsonConvert.DeserializeObject<SettingsJsonObject>(settingsJSONContent);

            if (convertedSettings is not null)
            {
                _settingsObject = convertedSettings;
            }
            else
            {
                throw new FileLoadException("Settings file could not be deserialized.");
            }

            return _settingsObject;
        }
    }
}
