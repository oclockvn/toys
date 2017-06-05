using NUnit.Framework;
using toys.Helpers;

namespace toys.test
{
    [TestFixture]
    public class MoneyExtensionTests
    {
        [Test]
        public void TestRoundMoney()
        {
            decimal d1 = 500.01m;
            decimal d2 = 1520.0m;
            decimal d3 = 22875.2m;

            int i1 = MoneyHelper.ToVnd(d1);
            int i2 = MoneyHelper.ToVnd(d2);
            int i3 = MoneyHelper.ToVnd(d3);

            Assert.AreEqual(500, i1);
            Assert.AreEqual(1500, i2);
            Assert.AreEqual(23000, i3);
        }
    }
}
