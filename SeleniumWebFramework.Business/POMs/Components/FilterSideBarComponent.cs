using OpenQA.Selenium;

namespace SeleniumWebFramework.Business.POMs.Components;


public class FilterSideBarComponent : BasePage
{
    #region Locators
    public By SortDropdown => By.CssSelector("[@data-test='sort-select']");
    public By SearchInput => By.XPath("//input[@data-test='search-query']");
    public By SearchButton => By.XPath("//button[@data-test='search-submit']");
    public By ResetButton => By.XPath("//button[@data-test='search-reset']");
    public By CategoryInput(string category) => By.XPath($"//label[contains(text(), '{category}')]//input");
    #endregion

    public void OpenSortDropdown() => Click(SortDropdown);
    public void SelectSortOption(string optionText) => SelectByText(SortDropdown, optionText);
    public void FillSearchInput(string searchText) => SendKeys(SearchInput, searchText);
    public void ClickSearchButton() => Click(SearchButton);
    public void ClickResetButton() => Click(ResetButton);
    
    public void SelectCategory(string category) => Click(CategoryInput(category));
}