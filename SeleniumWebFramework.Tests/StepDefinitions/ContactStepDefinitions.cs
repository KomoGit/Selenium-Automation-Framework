using Reqnroll;
using SeleniumWebFramework.Business.POMs;

namespace SeleniumWebFramework.Tests.StepDefinitions;

[Binding]
public class ContactStepDefinitions
{
    private readonly ContactPage _contactPage = new();

    [Given("I am on the contact page")]
    [Given("I navigate to the contact page")]
    public void GivenIAmOnTheContactPage()
    {
        _contactPage.Open();
    }

    [When("I fill out the contact form with first name {string}, last name {string}, email {string}, subject {string}, and message {string}")]
    public void WhenIFillOutTheContactForm(string firstName, string lastName, string email, string subject, string message)
    {
        _contactPage.FillForm(firstName, lastName, email, subject, message);
    }

    [When("I enter first name {string}")]
    public void WhenIEnterFirstName(string firstName)
    {
        _contactPage.FillFirstName(firstName);
    }

    [When("I enter last name {string}")]
    public void WhenIEnterLastName(string lastName)
    {
        _contactPage.FillLastName(lastName);
    }

    [When("I enter email {string}")]
    public void WhenIEnterEmail(string email)
    {
        _contactPage.FillEmail(email);
    }

    [When("I select subject {string}")]
    public void WhenISelectSubject(string subject)
    {
        _contactPage.SelectSubject(subject);
    }

    [When("I enter message {string}")]
    public void WhenIEnterMessage(string message)
    {
        _contactPage.FillMessage(message);
    }

    [When("I submit the contact form")]
    [When("I click the send button")]
    public void WhenISubmitTheContactForm()
    {
        _contactPage.ClickSend();
    }

    [Then("I should see the contact success message {string}")]
    public void ThenIShouldSeeTheContactSuccessMessage(string expectedMessage)
    {
        string actualMessage = _contactPage.GetSuccessAlertText();
        Assert.That(actualMessage, Does.Contain(expectedMessage));
    }

    [Then("I should see validation error {string} for {string} field")]
    [Then("I should see {string} error for {string}")]
    public void ThenIShouldSeeValidationErrorForField(string expectedError, string fieldName)
    {
        string actualError = _contactPage.GetFieldErrorText(fieldName);
        Assert.That(actualError, Does.Contain(expectedError));
    }
}
