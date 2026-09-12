using System.Text.Json;
using System.Runtime.InteropServices;

namespace ClickCore.Mac;

public sealed class MacSettings
{
    public string SelectedPage { get; set; } = "Clicker";
    public double ClicksPerSecond { get; set; } = 10;
    public bool AlwaysOnTop { get; set; }
    public string Theme { get; set; } = "Dark";
}

public static class SettingsStore
{
    private static readonly string DirectoryPath = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support", "ClickCore")
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClickCore");
    public static string FilePath => Path.Combine(DirectoryPath, "settings.json");

    public static MacSettings Load()
    {
        try { return File.Exists(FilePath) ? JsonSerializer.Deserialize<MacSettings>(File.ReadAllText(FilePath)) ?? new() : new(); }
        catch { return new(); }
    }

    public static void Save(MacSettings settings)
    {
        try
        {
            Directory.CreateDirectory(DirectoryPath);
            var temp = FilePath + ".tmp";
            File.WriteAllText(temp, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temp, FilePath, true);
        }
        catch { /* A read-only profile must not prevent the shell from starting. */ }
    }
}
