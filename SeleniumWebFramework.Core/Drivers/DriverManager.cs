using System.Diagnostics.CodeAnalysis;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using SeleniumWebFramework.Core.Models;

namespace SeleniumWebFramework.Core.Drivers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class DriverManager
{
    private static readonly AsyncLocal<IWebDriver?> _driver = new();
    public static IWebDriver Driver => _driver.Value ?? throw new InvalidOperationException("Driver is not initialized on the current async context.");
    
    /// <summary>
    /// Initialize an async context specific web driver (local or remote Selenium Grid) and add arguments.
    /// </summary>
    /// <param name="browser"></param>
    /// <param name="isHeadless"></param>
    /// <param name="implicitWaitSeconds"></param>
    /// <param name="gridOptions"></param>
    /// <param name="args"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void InitializeDriver(
        string browser = "chrome", 
        bool isHeadless = false, 
        int implicitWaitSeconds = 10, 
        GridConfigurationOptions? gridOptions = null, 
        params string[] args)
    {
        if (_driver.Value != null)
        {
            throw new InvalidOperationException("Driver is already initialized on this async context. Call Quit() before initializing a new instance.");
        }
        
        string[] safeArgs = args ?? [];
        string targetBrowser = (!string.IsNullOrWhiteSpace(gridOptions?.Browser) ? gridOptions.Browser : browser)?.Trim().ToLowerInvariant() ?? "chrome";
        string executionMode = gridOptions?.ExecutionMode?.Trim().ToLowerInvariant() ?? "local";
        string gridUrl = gridOptions?.GridUrl ?? "http://localhost:4444/wd/hub";

        DriverOptions options = GetDriverOptions(targetBrowser, isHeadless, safeArgs);

        IWebDriver driver = executionMode switch
        {
            "grid" or "remote" => CreateRemoteDriver(gridUrl, options),
            _ => CreateLocalDriver(targetBrowser, options)
        };

        if (implicitWaitSeconds > 0)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitSeconds);
        }

        try
        {
            driver.Manage().Window.Maximize();
        }
        catch
        {
            // Ignore window maximize failures in headless modes if not supported
        }

        _driver.Value = driver;
    }

    private static RemoteWebDriver CreateRemoteDriver(string gridUrl, DriverOptions options)
    {
        if (string.IsNullOrWhiteSpace(gridUrl))
        {
            throw new ArgumentException("Grid URL cannot be null or empty when ExecutionMode is set to 'grid' or 'remote'.", nameof(gridUrl));
        }

        return new RemoteWebDriver(new Uri(gridUrl), options);
    }

    private static IWebDriver CreateLocalDriver(string browser, DriverOptions options)
    {
        return browser switch
        {
            "chrome" => new ChromeDriver((ChromeOptions)options),
            "firefox" or "ff" => new FirefoxDriver((FirefoxOptions)options),
            "safari" or "webkit" => new SafariDriver((SafariOptions)options),
            _ => throw new ArgumentException($"Browser '{browser}' is not supported.", nameof(browser))
        };
    }

    private static DriverOptions GetDriverOptions(string browser, bool isHeadless, string[] args)
    {
        return browser switch
        {
            "chrome" => GetChromeOptions(isHeadless, args),
            "firefox" or "ff" => GetFirefoxOptions(isHeadless, args),
            "safari" or "webkit" => GetSafariOptions(isHeadless, args),
            _ => throw new ArgumentException($"Browser '{browser}' is not supported.", nameof(browser))
        };
    }

    private static ChromeOptions GetChromeOptions(bool isHeadless, params string[] args)
    {
        ChromeOptions chromeOptions = new();
        
        if (isHeadless)
        {
            chromeOptions.AddArgument("--headless=new");
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddAdditionalOption("useAutomationExtension", false);
        }
        
        chromeOptions.AddArguments(args);
        return chromeOptions;
    }

    private static FirefoxOptions GetFirefoxOptions(bool isHeadless, params string[] args)
    {
        FirefoxOptions firefoxOptions = new();
        
        if (isHeadless)
        {
            firefoxOptions.AddArgument("--headless");
            firefoxOptions.AddArgument("--width=1920");
            firefoxOptions.AddArgument("--height=1080");
        }
        
        firefoxOptions.AddArguments(args);
        return firefoxOptions;
    }

    private static SafariOptions GetSafariOptions(bool isHeadless, params string[] args)
    {
        SafariOptions safariOptions = new();
        
        if (isHeadless)
        {
            Console.WriteLine("[Warning] Safari does not support headless mode. Running in standard GUI mode.");
        }

        if (args.Length > 0)
        {
            Console.WriteLine("[Warning] SafariDriver does not support command-line arguments. Ignored passed args.");
        }
        
        return safariOptions;
    }
    
    public static void Quit()
    {
        _driver.Value?.Quit();
        _driver.Value?.Dispose();
        _driver.Value = null;
    }
}