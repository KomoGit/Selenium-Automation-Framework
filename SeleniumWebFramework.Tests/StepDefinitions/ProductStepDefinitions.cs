using Reqnroll;
using SeleniumWebFramework.Business.POMs;

namespace SeleniumWebFramework.Tests.StepDefinitions;

[Binding]
public class ProductStepDefinitions
{
    private readonly HomePage _homepage = new();


    [Then("I should see the search results for {string}")]
    public void ThenIShouldSeeTheSearchResultsFor(string searchTerm)
    {
        var products = _homepage.ProductCard(searchTerm).GetAllProducts();
        var productNames = _homepage.ProductCard(searchTerm).GetAllProductNames();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(products, Is.Not.Empty, $"No products found for search term: {searchTerm}");
            Assert.That(productNames, Has.Some.Contain(searchTerm), $"Search results do not contain the search term: {searchTerm}");
        }
    }
}