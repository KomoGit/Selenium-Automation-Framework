using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebFramework.Business.POMs.Components;
public class ProductCardComponent : BasePage
{
    private readonly string _productName;
    private readonly string _productCardXPath;

    public ProductCardComponent(string productName)
    {
        _productName = productName;
        _productCardXPath = $"//h5[contains(text(), '{_productName}')]";
    }

    public By ProductCardTitle => By.XPath(_productCardXPath);
    public By ProductCard => By.XPath($"{_productCardXPath}//ancestor::a[contains(@class, 'card')]");
    public By ProductCardPrice => By.XPath($"{_productCardXPath}//ancestor::a//span[@data-test='product-price']");
    public By ProductCardCORating => By.XPath($"{_productCardXPath}//ancestor::a//div[@data-test='co2-rating-badge']");
    public By ProductCompareButton => By.XPath($"{_productCardXPath}//ancestor::a//button[contains(@data-test, 'compare-btn')]");

    public void ClickProductCard() => Click(ProductCard);
    public void ClickProductCompareButton() => Click(ProductCompareButton);
    public ReadOnlyCollection<IWebElement> GetAllProducts() => Driver.FindElements(By.XPath("//a[contains(@class, 'card')]"));
    public string GetProductName() => Driver.FindElement(ProductCardTitle).Text;
    public List<string> GetAllProductNames(int timeoutInSeconds = 5)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        return wait.Until(d =>
        {
            var elements = d.FindElements(By.XPath("//a[contains(@class, 'card')]//h5"));
            var names = elements.Select(e => e.Text).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            return names.Count > 0 ? names : null;
        }) ?? [];
    }
    public string GetProductPrice() => Driver.FindElement(ProductCardPrice).Text;
    public string GetProductCORating() => Driver.FindElement(ProductCardCORating).Text;
}