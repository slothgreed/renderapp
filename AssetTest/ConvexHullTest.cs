using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset.Loader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssetTest
{
    class ConvexHullTest : ITestMethodInvoker
    {
        public TestMethodInvokerResult Invoke(params object[] parameters)
        {
            var stlModel = TestUtility.Load3DModel(TestData.STLFile) as STLLoader;
            HalfEdgeDS halfEdgeDS = new HalfEdgeDS("Test", stlModel.Position);

            var convex = new ConvexHullAlgorithm(halfEdgeDS.Vertexs);
            if (convex.Points.Count == 0)
            {
                Assert.Fail("Failed Create ConvexHull");
            }

            return new TestMethodInvokerResult();
        }
    }
}
