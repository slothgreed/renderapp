﻿using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    public class EnvironmentProbe : KIObject
    {
        public Texture Cubemap
        {
            get;
            private  set;
        }

        List<string> texturePath;

        public EnvironmentProbe(string name)
            : base(name)
        {
            texturePath = new List<string>();
        }
        public void GenCubemap(string[] paths)
        {
            if (paths.Length != 6)
            {
                Logger.Log(Logger.LogLevel.Error, "not set 6 paths ");
                return;
            }
            GenCubemap(paths[0], paths[1], paths[2], paths[3], paths[4], paths[5]);
        }
        public void GenCubemap(string px, string py, string pz, string nx, string ny, string nz)
        {
            Cubemap = TextureFactory.Instance.CreateCubemapTexture(px, py, pz, nx, ny, nz);
            texturePath.Add(px); texturePath.Add(py); texturePath.Add(pz);
            texturePath.Add(nx); texturePath.Add(ny); texturePath.Add(nz);
        }
    }
}