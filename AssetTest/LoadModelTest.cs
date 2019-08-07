using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTest
{
    public class LoadModelTest : ITestMethodInvoker
    {
        public TestMethodInvokerResult Invoke(params object[] parameters)
        {
            var filePaths = new string[]
                {
                    TestData.STLFile,
                    TestData.PLYFile
                };

            foreach (var filePath in filePaths)
            {
                var succes = TestUtility.Load3DModel(filePath);
                if (succes == null)
                {
                    Assert.Fail("Load Failed", filePath);
                }
            }

            return new TestMethodInvokerResult();
        }
    }
}
