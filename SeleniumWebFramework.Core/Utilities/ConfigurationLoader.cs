using SeleniumWebFramework.Core.Constants;
using SeleniumWebFramework.Core.Models;

namespace SeleniumWebFramework.Core.Utilities;

public abstract class ConfigurationLoader
{
    private static ConfigurationModel? _cachedInstance;
    private static readonly object _lock = new();

    public static ConfigurationModel LoadConfiguration(string fileName = "appsettings.json")
    {
        if (_cachedInstance == null)
        {
            lock (_lock)
            {
                _cachedInstance ??= ReadConfiguration(fileName);
            }
        }
        return _cachedInstance;
    }

    private static ConfigurationModel ReadConfiguration(string fileName)
    {
        ConfigurationModel model;
        var baseDir = PathUtils.GetBaseDirectory();
        var path = Path.Combine(baseDir, fileName);

        if (!File.Exists(path))
        {
            path = Path.Combine(PathUtils.GetProjectRoot(), fileName);
        }

        Console.WriteLine($"Loading configuration file {path}");

        try 
        {
            var json = File.ReadAllText(path);
            model = JsonUtils.Deserialize<ConfigurationModel>(json);
        }
        catch (Exception e)
        {
            throw new FormatException($"Could not load configuration file {path}", e);
        }

        string? envBaseUrl = Environment.GetEnvironmentVariable("BASE_URL");
        if (!string.IsNullOrWhiteSpace(envBaseUrl))
        {
            model.BaseUrl = envBaseUrl;
        }

        string? envBrowser = Environment.GetEnvironmentVariable("BROWSER");
        if (!string.IsNullOrWhiteSpace(envBrowser))
        {
            model.Browser = envBrowser;
            if (model.GridConfigurationOptions != null)
            {
                model.GridConfigurationOptions.Browser = envBrowser;
            }
        }

        string? envExecMode = Environment.GetEnvironmentVariable("EXECUTION_MODE");
        if (!string.IsNullOrWhiteSpace(envExecMode) && model.GridConfigurationOptions != null)
        {
            model.GridConfigurationOptions.ExecutionMode = envExecMode;
        }

        string? envGridUrl = Environment.GetEnvironmentVariable("GRID_URL") ?? Environment.GetEnvironmentVariable("SELENIUM_GRID_URL");
        if (!string.IsNullOrWhiteSpace(envGridUrl) && model.GridConfigurationOptions != null)
        {
            model.GridConfigurationOptions.GridUrl = envGridUrl;
        }

        return model;
    }
}