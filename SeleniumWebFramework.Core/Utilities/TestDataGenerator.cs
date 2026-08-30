using Bogus;

namespace SeleniumWebFramework.Core.Utilities;

/// <summary>
/// Model representing customer account details.
/// </summary>
public record CustomerData(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Phone,
    DateTime DateOfBirth,
    AddressData Address
);

/// <summary>
/// Model representing physical shipping or billing addresses.
/// </summary>
public record AddressData(
    string StreetAddress,
    string ApartmentOrSuite,
    string City,
    string State,
    string ZipCode,
    string Country,
    string CountryCode
);

/// <summary>
/// Model representing payment method details.
/// </summary>
public record PaymentData(
    string CardNumber,
    string CardHolderName,
    string ExpirationDate,
    string Cvv,
    string CardType
);

/// <summary>
/// Model representing contact form submission payload.
/// </summary>
public record ContactFormData(
    string FirstName,
    string LastName,
    string Email,
    string Subject,
    string Message
);

/// <summary>
/// Model representing e-commerce product items.
/// </summary>
public record ProductData(
    string ProductName,
    string Category,
    decimal Price,
    int Quantity,
    string Sku
);

/// <summary>
/// Thread-safe test data generator utility powered by Bogus for E-Commerce automation scenarios.
/// </summary>
public static class TestDataGenerator
{
    private static readonly Faker DefaultFaker = new("en");

    /// <summary>
    /// Generates a realistic customer profile complete with address.
    /// </summary>
    public static CustomerData GenerateCustomer(string locale = "en")
    {
        var faker = GetFaker(locale);
        var address = GenerateAddress(locale);

        return new CustomerData(
            FirstName: faker.Name.FirstName(),
            LastName: faker.Name.LastName(),
            Email: faker.Internet.Email().ToLowerInvariant(),
            Password: faker.Internet.Password(12, false, "", "@1Aa"),
            Phone: faker.Phone.PhoneNumber("###-###-####"),
            DateOfBirth: faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Address: address
        );
    }

    /// <summary>
    /// Generates physical address details.
    /// </summary>
    public static AddressData GenerateAddress(string locale = "en")
    {
        var faker = GetFaker(locale);

        return new AddressData(
            StreetAddress: faker.Address.StreetAddress(),
            ApartmentOrSuite: faker.Address.SecondaryAddress(),
            City: faker.Address.City(),
            State: faker.Address.State(),
            ZipCode: faker.Address.ZipCode(),
            Country: faker.Address.Country(),
            CountryCode: faker.Address.CountryCode()
        );
    }

    /// <summary>
    /// Generates credit card payment details for checkout scenarios.
    /// </summary>
    public static PaymentData GeneratePayment(string locale = "en")
    {
        var faker = GetFaker(locale);

        return new PaymentData(
            CardNumber: faker.Finance.CreditCardNumber(),
            CardHolderName: $"{faker.Name.FirstName()} {faker.Name.LastName()}".ToUpperInvariant(),
            ExpirationDate: $"{faker.Date.Future().Month:D2}/{faker.Date.Future().Year % 100:D2}",
            Cvv: faker.Finance.CreditCardCvv(),
            CardType: faker.PickRandom("Visa", "MasterCard", "American Express", "Discover")
        );
    }

    /// <summary>
    /// Generates contact form data with configurable minimum message length.
    /// </summary>
    public static ContactFormData GenerateContactForm(int minMessageLength = 55, string locale = "en")
    {
        var faker = GetFaker(locale);

        string message = faker.Lorem.Paragraph(3);
        while (message.Length < minMessageLength)
        {
            message += " " + faker.Lorem.Sentence();
        }

        return new ContactFormData(
            FirstName: faker.Name.FirstName(),
            LastName: faker.Name.LastName(),
            Email: faker.Internet.Email().ToLowerInvariant(),
            Subject: faker.PickRandom(new[] { "Customer service", "Webmaster", "Payments", "Warranty", "Returns" }),
            Message: message
        );
    }

    /// <summary>
    /// Generates mock product inventory data.
    /// </summary>
    public static ProductData GenerateProduct(string locale = "en")
    {
        var faker = GetFaker(locale);

        return new ProductData(
            ProductName: faker.Commerce.ProductName(),
            Category: faker.Commerce.Categories(1)[0],
            Price: decimal.Parse(faker.Commerce.Price(10, 500, 2)),
            Quantity: faker.Random.Number(1, 10),
            Sku: faker.Commerce.Ean8()
        );
    }

    private static Faker GetFaker(string locale)
    {
        return string.Equals(locale, "en", StringComparison.OrdinalIgnoreCase)
            ? DefaultFaker
            : new Faker(locale);
    }
}
