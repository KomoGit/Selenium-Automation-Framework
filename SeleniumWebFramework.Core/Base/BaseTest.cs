using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumWebFramework.Core.Drivers;
using SeleniumWebFramework.Core.Models;
using SeleniumWebFramework.Core.Utilities;

namespace SeleniumWebFramework.Core.Base;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public abstract class BaseTest
{
    protected ConfigurationModel Config { get; private set; }
    protected IWebDriver Driver { get; private set; }
    
    [SetUp]
    protected virtual void Setup()
    {
        Config = ConfigurationModel.Instance;
        
        DriverManager
            .InitializeDriver(
                Config.Browser, 
                Config.IsHeadless, 
                Config.ImplicitWait,
                Config.GridConfigurationOptions,
                Config.DriverConfigurationOptions?.Params ?? Array.Empty<string>());

        Driver = DriverManager.Driver;
        Driver.Navigate().GoToUrl(Config.BaseUrl);
    }

    [TearDown]
    protected void TearDown()
    {
        DriverManager.Quit();
    }
}