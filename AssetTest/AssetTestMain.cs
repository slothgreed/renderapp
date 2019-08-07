using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssetTest
{
    [TestClass]
    public class AssetTestMain
    {
        [TestMethod]
        public void AssetTestMainEntry()
        {
            ITestMethodInvoker[] testMethod = new ITestMethodInvoker[]
                {
                    new LoadModelTest(),
                    new ConvexHullTest()
                };

            foreach (var test in testMethod)
            {
                var result = test.Invoke(null);
                if (result == null)
                {
                    Assert.Fail("Test Class Error");
                }
            }
        }
    }
}
