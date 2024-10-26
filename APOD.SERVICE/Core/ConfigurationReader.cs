namespace APOD.SERVICE.Core;

internal static class ConfigurationReader
{
    internal static string ReadSetting(string key)
    {
        const string configFilePath = "config.ini";

        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        var lines = File.ReadAllLines(configFilePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("["))
                continue;

            var keyValue = line.Split('=');

            if (keyValue[0].Trim() == key)
            {
                return keyValue[1].Trim();
            }
        }

        throw new KeyNotFoundException($"Key '{key}' not found in configuration.");
    }
}
