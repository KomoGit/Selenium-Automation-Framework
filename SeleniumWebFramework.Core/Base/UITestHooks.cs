using Allure.Net.Commons;
using OpenQA.Selenium;
using Reqnroll;
using SeleniumWebFramework.Core.Drivers;
using SeleniumWebFramework.Core.Models;
using SeleniumWebFramework.Core.Utilities;

namespace SeleniumWebFramework.Core.Base;

[Binding]
public class UITestHooks
{
    private readonly ConfigurationModel _config = ConfigurationModel.Instance;
    private readonly ScenarioContext _scenarioContext;

    public UITestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario("ui", Order = 0)]
    public void BeforeScenario()
    {
        DriverManager.InitializeDriver(
            _config.Browser, 
            _config.IsHeadless, 
            _config.ImplicitWait,
            _config.GridConfigurationOptions,
            _config.DriverConfigurationOptions?.Params ?? Array.Empty<string>()
        );
    }

    [AfterScenario("ui")]
    public void AfterScenario()
    {
        try
        {
            if (_scenarioContext.TestError != null && DriverManager.Driver is ITakesScreenshot ts)
            {
                byte[] screenshotBytes = ts.GetScreenshot().AsByteArray;
                AllureApi.AddAttachment(
                    $"Screenshot Failure - {_scenarioContext.ScenarioInfo.Title}",
                    "image/png",
                    screenshotBytes,
                    ".png"
                );
            }
        }
        catch
        {
            // Ignore screenshot attachment failure during teardown
        }
        finally
        {
            DriverManager.Quit();
        }
    }
}