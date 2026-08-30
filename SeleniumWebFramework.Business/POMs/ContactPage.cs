using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumWebFramework.Business.POMs
{
    public class ContactPage : BasePage
    {
        public By PageTitle => By.XPath("//h3[text()='Contact']");
        public By FirstNameInput => By.CssSelector("#first_name, [data-testid='first-name']");
        public By LastNameInput => By.CssSelector("#last_name, [data-testid='last-name']");
        public By EmailInput => By.CssSelector("#email, [data-testid='email']");
        public By FileUpload => By.CssSelector("#attachment, [data-testid='attachment']");
        public By AttachmentError => By.CssSelector("[data-testid='attachment-error']");
        public By SubjectInput => By.CssSelector("#subject, [data-testid='subject']");
        public By MessageInput => By.CssSelector("#message, [data-testid='message']");
        public By SendButton => By.CssSelector(".btnSubmit, [data-testid='contact-submit'], [value='Send']");
        public By SuccessAlert => By.CssSelector(".alert-success, [data-testid='alert'], .alert");

        public By FirstNameError => By.CssSelector("#first_name_alert, [data-testid='first-name-error']");
        public By LastNameError => By.CssSelector("#last_name_alert, [data-testid='last-name-error']");
        public By EmailError => By.CssSelector("#email_alert, [data-testid='email-error']");
        public By SubjectError => By.CssSelector("#subject_alert, [data-testid='subject-error']");
        public By MessageError => By.CssSelector("#message_alert, [data-testid='message-error']");

        public void Open()
        {
            NavigateToPath("contact");
        }

        public void FillFirstName(string firstName) => SendKeys(FirstNameInput, firstName);
        public void FillLastName(string lastName) => SendKeys(LastNameInput, lastName);
        public void FillEmail(string email) => SendKeys(EmailInput, email);
        public void SelectSubject(string subjectText) => SendKeys(SubjectInput, subjectText);
        public void FillMessage(string message) => SendKeys(MessageInput, message);

        public void FillForm(string firstName, string lastName, string email, string subject, string message)
        {
            FillFirstName(firstName);
            FillLastName(lastName);
            FillEmail(email);
            SelectSubject(subject);
            FillMessage(message);
        }

        public void ClickSend() => Click(SendButton);

        public string GetSuccessAlertText(int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait.Until(d =>
            {
                var element = d.FindElement(SuccessAlert);
                if (element != null && element.Displayed && !string.IsNullOrWhiteSpace(element.Text))
                {
                    return element.Text;
                }
                return null;
            }) ?? throw new WebDriverTimeoutException($"Success alert text not found within {timeoutInSeconds} seconds.");
        }

        public string GetFieldErrorText(string fieldName, int timeoutInSeconds = 5)
        {
            By locator = fieldName.ToLowerInvariant().Replace(" ", "").Replace("-", "").Replace("_", "") switch
            {
                "firstname" or "first_name" or "first-name" => FirstNameError,
                "lastname" or "last_name" or "last-name" => LastNameError,
                "email" => EmailError,
                "subject" => SubjectError,
                "message" => MessageError,
                _ => throw new ArgumentException($"Unknown field name: {fieldName}")
            };

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait.Until(d =>
            {
                var element = d.FindElement(locator);
                if (element != null && element.Displayed && !string.IsNullOrWhiteSpace(element.Text))
                {
                    return element.Text;
                }
                return null;
            }) ?? throw new WebDriverTimeoutException($"Validation error text for field '{fieldName}' not found within {timeoutInSeconds} seconds.");
        }
    }
}