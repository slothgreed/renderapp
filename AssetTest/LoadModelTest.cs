using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KI.Asset.Loader.Model;
using KI.Asset.Loader;

namespace AssetTest
{
    [TestClass]
    public class LoadModelTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var filePaths = new string[]
                {
                    @"E:\MyProgram\KIProject\renderapp\Resource\Model\fandisk.stl",
                    @"E:\MyProgram\KIProject\renderapp\Resource\Model\sphere1.ply"
                };

            foreach (var filePath in filePaths)
            {
               var succes = Load3DModel(filePath);
                if (succes == false)
                {
                    Assert.Fail("Load Failed", filePath);
                }
            }
        }

        public bool Load3DModel(string filePath)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);
                IModelLoader loader;
                switch (extension)
                {
                    case ".stl":
                        loader = new STLLoader(filePath);
                        break;
                    case ".ply":
                        loader = new PLYLoader(filePath);
                        break;
                    case ".off":
                        loader = new OFFLoader(filePath);
                        break;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
