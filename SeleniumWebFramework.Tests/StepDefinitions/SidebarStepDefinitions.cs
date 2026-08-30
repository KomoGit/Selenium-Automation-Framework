using Reqnroll;
using SeleniumWebFramework.Business.POMs;

namespace SeleniumWebFramework.Tests.StepDefinitions;

[Binding]
public class SidebarStepDefinitions
{
    private readonly HomePage _homepage = new();

    [When("I fill out the search bar with {string}")]
    public void WhenIFillOutTheSearchBarWith(string searchTerm)
    {
        _homepage.FilterSideBar.FillSearchInput(searchTerm);
    }

    [When("I click the search button")]
    public void WhenIClickTheSearchButton()
    {
        _homepage.FilterSideBar.ClickSearchButton();
    }

    [When("I click the clear button")]
    public void WhenIClickTheClearButton()
    {
        _homepage.FilterSideBar.ClickResetButton();
    }

    [When("I select the {string} category")]
    public void WhenISelectTheCategory(string category)
    {
        _homepage.FilterSideBar.SelectCategory(category);
    }

    [When("I select the {string} sort option")]
    public void WhenISelectTheSortOption(string sortOption)
    {
        _homepage.FilterSideBar.SelectSortOption(sortOption);
    }

    [When("I open the sort dropdown")]
    public void WhenIOpenTheSortDropdown()
    {
        _homepage.FilterSideBar.OpenSortDropdown();
    }
}