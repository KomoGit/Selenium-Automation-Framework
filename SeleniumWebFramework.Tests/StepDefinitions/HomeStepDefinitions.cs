using Reqnroll;
using SeleniumWebFramework.Business.POMs;

namespace SeleniumWebFramework.Tests.StepDefinitions;

[Binding]

public class HomeStepDefinitions
{
    private readonly HomePage _homepage = new();

    [Given("I am on the home page")]
    [Given("I navigate to the home page")]
    public void GivenIAmOnTheHomePage()
    {
        _homepage.Open();
    }
}