using Reqnroll;
using SeleniumWebFramework.Business.POMs;

namespace SeleniumWebFramework.Tests.StepDefinitions;

[Binding]
public class TabStepDefinitions : BasePage
{
    #region Open Tab Steps

    /// <summary>
    /// Opens a new blank browser tab.
    /// Example: When I open a new tab
    /// </summary>
    [Given("I open a new tab")]
    [When("I open a new tab")]
    public void GivenIOpenANewTab()
    {
        OpenNewTab();
    }

    /// <summary>
    /// Opens a new browser tab and navigates to the specified URL.
    /// Example: When I open a new tab to "https://www.google.com"
    /// </summary>
    [Given("I open a new tab to {string}")]
    [When("I open a new tab to {string}")]
    public void GivenIOpenANewTabToUrl(string url)
    {
        OpenNewTab(url);
    }

    #endregion

    #region Switch Tab Steps

    /// <summary>
    /// Switches to the tab by 1-based index (e.g., tab 1 for the first tab).
    /// Example: When I switch to tab 2
    /// </summary>
    [When("I switch to tab {int}")]
    public void WhenISwitchToTabNumber(int tabNumber)
    {
        int index = tabNumber > 0 ? tabNumber - 1 : 0;
        SwitchToTab(index);
    }

    /// <summary>
    /// Switches to the first/main browser tab.
    /// Example: When I switch to the first tab
    /// </summary>
    [When("I switch to the first tab")]
    [When("I switch to the main tab")]
    public void WhenISwitchToTheFirstTab()
    {
        SwitchToTab(0);
    }

    /// <summary>
    /// Switches to a tab matching the given title or URL keyword.
    /// Example: When I switch to tab containing "Google"
    /// </summary>
    [When("I switch to tab containing {string}")]
    [When("I switch to tab with title {string}")]
    public void WhenISwitchToTabContaining(string titleOrUrl)
    {
        SwitchToTab(titleOrUrl);
    }

    /// <summary>
    /// Switches to the next tab.
    /// Example: When I switch to the next tab
    /// </summary>
    [When("I switch to the next tab")]
    public void WhenISwitchToNextTab()
    {
        var handles = Driver.WindowHandles;
        int currentIndex = handles.IndexOf(Driver.CurrentWindowHandle);
        int nextIndex = (currentIndex + 1) % handles.Count;
        SwitchToTab(nextIndex);
    }

    /// <summary>
    /// Switches to the previous tab.
    /// Example: When I switch to the previous tab
    /// </summary>
    [When("I switch to the previous tab")]
    public void WhenISwitchToPreviousTab()
    {
        var handles = Driver.WindowHandles;
        int currentIndex = handles.IndexOf(Driver.CurrentWindowHandle);
        int prevIndex = (currentIndex - 1 + handles.Count) % handles.Count;
        SwitchToTab(prevIndex);
    }

    #endregion

    #region Close Tab Steps

    /// <summary>
    /// Closes the currently active browser tab.
    /// Example: When I close the current tab
    /// </summary>
    [When("I close the current tab")]
    [When("I close the active tab")]
    public void WhenICloseTheCurrentTab()
    {
        CloseCurrentTab();
    }

    /// <summary>
    /// Closes the tab by 1-based index.
    /// Example: When I close tab 2
    /// </summary>
    [When("I close tab {int}")]
    public void WhenICloseTabNumber(int tabNumber)
    {
        int index = tabNumber > 0 ? tabNumber - 1 : 0;
        CloseTab(index);
    }

    /// <summary>
    /// Closes all open tabs except the currently active tab.
    /// Example: When I close all other tabs
    /// </summary>
    [When("I close all other tabs")]
    public void WhenICloseAllOtherTabs()
    {
        string currentHandle = Driver.CurrentWindowHandle;
        var handles = Driver.WindowHandles;

        foreach (string handle in handles)
        {
            if (handle != currentHandle)
            {
                Driver.SwitchTo().Window(handle);
                Driver.Close();
            }
        }

        Driver.SwitchTo().Window(currentHandle);
    }

    #endregion
}
