using IPhoneStockChecker.Notifiers.ServiceConfigurations;

namespace IPhoneStockChecker.Notifiers.Test.ServiceConfigurations;

[TestClass]
public class NotifierServiceComponentTest
{
    [TestMethod]
    public void Verify_ShouldSucceed()
    {
        new NotifierServiceComponent().Verify();
    }
}
