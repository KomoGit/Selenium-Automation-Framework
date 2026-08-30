using OpenQA.Selenium;

namespace SeleniumWebFramework.Business.POMs;

public class HeaderComponent : BasePage
{
    #region Elements
    public By HeaderLogo => By.XPath("//a[@title='Practice Software Testing - Toolshop']");
    public By HomeButton => By.CssSelector("[@data-test='nav-home']");
    public By CategoriesDropdown => By.XPath("[@data-test='nav-categories']");
    public By ContactButton => By.XPath("[@data-test='nav-link']");
    public By SignInButton => By.XPath("[@data-test='nav-sign-in']");
    public By LanguageDropdown => By.XPath("[@data-test='language-select']");
    #endregion

    public void ClickHomeButton() => Click(HomeButton);
    public void ClickContactButton() => Click(ContactButton);
    public void ClickSignInButton() => Click(SignInButton);
    public void ClickLanguageDropdown() => Click(LanguageDropdown);
    public void ClickCategoriesDropdown() => Click(CategoriesDropdown);
    public void ClickHeaderLogo() => Click(HeaderLogo);
    public void SelectCategory(string categoryName)
    {
        var categoryOption = By.XPath($"//a[@data-test='nav-category' and text()='{categoryName}']");
        Click(categoryOption);
    }

    public void SelectLanguage(string language)
    {
        var languageOption = By.XPath($"//option[@value='{language}']");
        Click(languageOption);
    }
}