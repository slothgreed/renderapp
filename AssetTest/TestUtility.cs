using KI.Asset.Loader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTest
{
    public static class TestUtility
    {
        [TestMethod]
        public static IModelLoader Load3DModel(string filePath)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);
                switch (extension)
                {
                    case ".stl":
                        return new STLLoader(filePath);
                    case ".ply":
                        return new PLYLoader(filePath);
                    case ".off":
                        return new OFFLoader(filePath);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
