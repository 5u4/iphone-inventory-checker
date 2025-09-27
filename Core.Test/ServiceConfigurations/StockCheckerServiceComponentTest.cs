using IPhoneStockChecker.Core.ServiceConfigurations;

namespace Core.Test.ServiceConfigurations;

[TestClass]
public class StockCheckerServiceComponentTest
{
    [TestMethod]
    public void Verify_ShouldSucceed()
    {
        new StockCheckerServiceComponent().Verify();
    }
}
