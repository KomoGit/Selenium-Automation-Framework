using System.Collections.ObjectModel;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumWebFramework.Core.Drivers;

namespace SeleniumWebFramework.Business.POMs;

public abstract class BasePage
{
    private readonly IWebDriver? _driver;

    protected IWebDriver Driver => _driver ?? DriverManager.Driver;

    protected BasePage(IWebDriver? driver = null)
    {
        _driver = driver;
    }

    /// <summary>
    /// Navigates to a path relative to the configured BaseUrl.
    /// </summary>
    /// <param name="path">Relative URL path (e.g., "contact").</param>
    protected void NavigateToPath(string path = "")
    {
        var config = SeleniumWebFramework.Core.Models.ConfigurationModel.Instance;
        string baseUrl = config.BaseUrl?.TrimEnd('/') ?? "https://practicesoftwaretesting.com";
        string targetUrl = string.IsNullOrWhiteSpace(path)
            ? baseUrl
            : $"{baseUrl}/{path.TrimStart('/')}";
        Driver.Navigate().GoToUrl(targetUrl);
    }

    #region Click Methods

    /// <summary>
    /// Clicks on an element located by <paramref name="locator"/> using a polling mechanism with explicit wait.
    /// Ignores transient exceptions such as <see cref="NoSuchElementException"/> and <see cref="ElementClickInterceptedException"/>.
    /// </summary>
    /// <param name="locator">The <see cref="By"/> locator of the element to click.</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 5 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 3 retries).</param>
    protected void Click(By locator, int timeoutInSeconds = 10, int retryCount = 5)
    {
        Click(() => Driver.FindElement(locator), timeoutInSeconds, retryCount);
    }

    /// <summary>
    /// Clicks on the specified <paramref name="element"/> using a polling mechanism with explicit wait.
    /// Ignores transient exceptions such as <see cref="NoSuchElementException"/> and <see cref="ElementClickInterceptedException"/>.
    /// </summary>
    /// <param name="element">The <see cref="IWebElement"/> to click.</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 10 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 5 retries).</param>
    protected void Click(IWebElement element, int timeoutInSeconds = 10, int retryCount = 5)
    {
        Click(() => element, timeoutInSeconds, retryCount);
    }

    /// <summary>
    /// Executes a click operation using a supplier function with polling retries and explicit wait timer.
    /// Ignores common transient exceptions such as <see cref="NoSuchElementException"/>, <see cref="ElementClickInterceptedException"/>,
    /// <see cref="StaleElementReferenceException"/>, and <see cref="ElementNotInteractableException"/>.
    /// </summary>
    /// <param name="elementSupplier">Function returning the web element to click.</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 10 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 5 retries).</param>
    protected void Click(Func<IWebElement> elementSupplier, int timeoutInSeconds = 10, int retryCount = 5)
    {
        int pollingMs = Math.Max(50, (int)((timeoutInSeconds * 1000.0) / Math.Max(1, retryCount)));
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds))
        {
            PollingInterval = TimeSpan.FromMilliseconds(pollingMs)
        };
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(ElementClickInterceptedException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException),
            typeof(NotFoundException)
        );

        wait.Until(d =>
        {
            IWebElement element = elementSupplier();
            element.Click();
            return true;
        });
    }

    #endregion

    #region Input Methods

    /// <summary>
    /// Enters text into an element located by <paramref name="locator"/> using a polling mechanism and explicit wait.
    /// </summary>
    /// <param name="locator">The <see cref="By"/> locator of the input element.</param>
    /// <param name="text">The text string to enter.</param>
    /// <param name="clearFirst">Whether to clear the input field before entering text (default is true).</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 10 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 5 retries).</param>
    protected void SendKeys(By locator, string text, bool clearFirst = true, int timeoutInSeconds = 10, int retryCount = 5)
    {
        SendKeys(() => Driver.FindElement(locator), text, clearFirst, timeoutInSeconds, retryCount);
    }

    /// <summary>
    /// Enters text into the specified <paramref name="element"/> using a polling mechanism and explicit wait.
    /// </summary>
    /// <param name="element">The <see cref="IWebElement"/> to enter text into.</param>
    /// <param name="text">The text string to enter.</param>
    /// <param name="clearFirst">Whether to clear the input field before entering text (default is true).</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 10 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 5 retries).</param>
    protected void SendKeys(IWebElement element, string text, bool clearFirst = true, int timeoutInSeconds = 10, int retryCount = 5)
    {
        SendKeys(() => element, text, clearFirst, timeoutInSeconds, retryCount);
    }

    /// <summary>
    /// Executes a smart input operation using a supplier function with polling retries and explicit wait timer.
    /// Ignores common transient exceptions such as <see cref="NoSuchElementException"/> and <see cref="ElementNotInteractableException"/>.
    /// </summary>
    /// <param name="elementSupplier">Function returning the target web element.</param>
    /// <param name="text">The text string to enter.</param>
    /// <param name="clearFirst">Whether to clear the input field before entering text (default is true).</param>
    /// <param name="timeoutInSeconds">Explicit wait timeout in seconds (default is 10 seconds).</param>
    /// <param name="retryCount">Number of polling retries (default is 5 retries).</param>
    protected void SendKeys(Func<IWebElement> elementSupplier, string text, bool clearFirst = true, int timeoutInSeconds = 10, int retryCount = 5)
    {
        int pollingMs = Math.Max(50, (int)((timeoutInSeconds * 1000.0) / Math.Max(1, retryCount)));
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds))
        {
            PollingInterval = TimeSpan.FromMilliseconds(pollingMs)
        };
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(ElementClickInterceptedException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException),
            typeof(NotFoundException)
        );

        wait.Until(d =>
        {
            IWebElement element = elementSupplier();
            if (clearFirst && !element.TagName.Equals("select", StringComparison.OrdinalIgnoreCase))
            {
                try { element.Clear(); } catch { }
            }
            element.SendKeys(text);
            return true;
        });
    }

    /// <summary>
    /// Alias for <see cref="SendKeys(By, string, bool, int, int)"/>.
    /// </summary>
    protected void EnterText(By locator, string text, bool clearFirst = true, int timeoutInSeconds = 5, int retryCount = 3)
    {
        SendKeys(locator, text, clearFirst, timeoutInSeconds, retryCount);
    }

    /// <summary>
    /// Alias for <see cref="SendKeys(IWebElement, string, bool, int, int)"/>.
    /// </summary>
    protected void EnterText(IWebElement element, string text, bool clearFirst = true, int timeoutInSeconds = 5, int retryCount = 3)
    {
        SendKeys(element, text, clearFirst, timeoutInSeconds, retryCount);
    }

    #endregion

    #region Tab Methods

    /// <summary>
    /// Opens a new browser tab and optionally navigates to the specified URL.
    /// </summary>
    /// <param name="url">Optional URL to navigate to in the new tab.</param>
    protected void OpenNewTab(string? url = null)
    {
        Driver.SwitchTo().NewWindow(WindowType.Tab);
        if (!string.IsNullOrWhiteSpace(url))
        {
            Driver.Navigate().GoToUrl(url);
        }
    }

    /// <summary>
    /// Switches to the tab at the specified 0-based index.
    /// </summary>
    /// <param name="index">0-based index of the tab.</param>
    protected void SwitchToTab(int index)
    {
        ReadOnlyCollection<string> handles = Driver.WindowHandles;
        if (index < 0 || index >= handles.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Tab index '{index}' is out of range. Total tabs: {handles.Count}");
        }
        Driver.SwitchTo().Window(handles[index]);
    }

    /// <summary>
    /// Switches to the tab matching the given title or URL substring.
    /// </summary>
    /// <param name="titleOrUrlSubstring">Substring to match against tab title or URL.</param>
    protected void SwitchToTab(string titleOrUrlSubstring)
    {
        ReadOnlyCollection<string> handles = Driver.WindowHandles;
        foreach (string handle in handles)
        {
            Driver.SwitchTo().Window(handle);
            if (Driver.Title.Contains(titleOrUrlSubstring, StringComparison.OrdinalIgnoreCase) ||
                Driver.Url.Contains(titleOrUrlSubstring, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }
        throw new NoSuchWindowException($"No tab found matching title or URL: '{titleOrUrlSubstring}'");
    }

    /// <summary>
    /// Closes the currently active tab and switches to the last remaining tab (if any).
    /// </summary>
    protected void CloseCurrentTab()
    {
        Driver.Close();
        ReadOnlyCollection<string> handles = Driver.WindowHandles;
        if (handles.Count > 0)
        {
            Driver.SwitchTo().Window(handles.Last());
        }
    }

    /// <summary>
    /// Closes the tab at the specified 0-based index.
    /// </summary>
    /// <param name="index">0-based index of the tab to close.</param>
    protected void CloseTab(int index)
    {
        ReadOnlyCollection<string> handles = Driver.WindowHandles;
        if (index < 0 || index >= handles.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Tab index '{index}' is out of range. Total tabs: {handles.Count}");
        }
        Driver.SwitchTo().Window(handles[index]);
        CloseCurrentTab();
    }

    /// <summary>
    /// Selects an option from a dropdown located by <paramref name="locator"/> by visible text using explicit wait.
    /// </summary>
    protected void SelectByText(By locator, string optionText, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException)
        );

        wait.Until(d =>
        {
            var selectElement = new SelectElement(d.FindElement(locator));
            selectElement.SelectByText(optionText);
            return true;
        });
    }

    /// <summary>
    /// Selects an option from a dropdown located by <paramref name="locator"/> by value attribute using explicit wait.
    /// </summary>
    protected void SelectByValue(By locator, string value, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException)
        );

        wait.Until(d =>
        {
            var selectElement = new SelectElement(d.FindElement(locator));
            selectElement.SelectByValue(value);
            return true;
        });
    }

    #endregion

    #region Helper Methods

    private static bool IsIgnoredException(Exception ex)
    {
        Exception current = ex;
        if (current is TargetInvocationException && current.InnerException != null)
        {
            current = current.InnerException;
        }

        return current is NoSuchElementException ||
               current is ElementClickInterceptedException ||
               current is StaleElementReferenceException ||
               current is ElementNotInteractableException ||
               current is NotFoundException;
    }

    #endregion
}