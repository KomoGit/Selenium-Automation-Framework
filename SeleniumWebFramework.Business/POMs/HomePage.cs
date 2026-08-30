using OpenQA.Selenium;
using SeleniumWebFramework.Business.POMs.Components;

namespace SeleniumWebFramework.Business.POMs;

public class HomePage : BasePage
{
    public ProductCardComponent ProductCard(string productName) => new(productName);
    public FilterSideBarComponent FilterSideBar => new();

    public By PageTitle => By.XPath("//a[@title='Practice Software Testing - Toolshop']");
    

    public void Open()
    {
        NavigateToPath();
    }
}