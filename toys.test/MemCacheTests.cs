using NUnit.Framework;
using System.Threading;
using toys.Helpers;

namespace toys.test
{
    [TestFixture]
    public class MemCacheTests
    {
        [Test]
        public void TestGetCorrentCacheValue()
        {
            ICache cache = new MemCache();
            string text = cache.GetOrSet("text", () => "hello world", 1);

            Assert.AreEqual("hello world", text);
        }

        [Test]
        public void TestGetCacheWithExpireTime()
        {
            ICache cache = new MemCache();
            string text = cache.GetOrSet("text", () => "hello world", 1);

            Thread.Sleep(63000);

            Assert.AreEqual("hello world", text); // should throw exception
        }
    }
}
