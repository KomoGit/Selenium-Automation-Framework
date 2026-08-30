using NUnit.Framework;
using SeleniumWebFramework.Core.Models;
using SeleniumWebFramework.Core.Utilities;

namespace SeleniumWebFramework.Tests;

[TestFixture]
public class ConfigurationModelTests
{
    [Test]
    public void Instance_ShouldReturnSameSingletonReference()
    {
        var instance1 = ConfigurationModel.Instance;
        var instance2 = ConfigurationModel.Instance;
        var instance3 = ConfigurationLoader.LoadConfiguration();

        Assert.That(instance1, Is.Not.Null);
        Assert.That(instance2, Is.SameAs(instance1));
        Assert.That(instance3, Is.SameAs(instance1));
    }
}
