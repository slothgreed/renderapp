using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
using KI.Gfx.KIImage;
using KI.Gfx.KIAsset;
namespace KI.Gfx.KIAsset
{
    public class TextureFactory
    {

        private static TextureFactory _instance = new TextureFactory();
        public static TextureFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public Dictionary<string, Texture> TextureList = new Dictionary<string, Texture>();
        public Texture CreateTexture(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            extension = extension.ToLower();
            ImageKind kind = ImageKind.None;
            switch (extension)
            {
                case ".bmp":
                    kind = ImageKind.BMP;
                    break;
                case ".png":
                    kind = ImageKind.PNG;
                    break;
                case ".jpg":
                    kind = ImageKind.JPG;
                    break;
                case ".tga":
                    kind = ImageKind.TGA;
                    break;
                case ".hdr":
                    kind = ImageKind.HDR;
                    break;
            }

            return CreateTexture(path, kind);
        }
        public Texture CreateTexture(string path, ImageKind kind)
        {
            if(TextureList.ContainsKey(path))
            {
                return TextureList[path];
            }
            KIImageInfo image = null;
            switch (kind)
            {
                case ImageKind.PNG:
                case ImageKind.JPG:
                case ImageKind.BMP:
                    image = new KIImageInfo(path);
                    break;
                case ImageKind.TGA:
                    image = new TGAImage(path);
                    break;
                case ImageKind.HDR:
                    image = new HDRImage(path);
                    break;
                default:
                    return null;
            }
            Texture texture = new Texture(KIFile.GetNameFromPath(path), path);
            TextureList.Add(path, texture);
            texture.LoadTexture(image);
            return texture;
        }
        public Texture CreateTexture(string name, int width, int height)
        {
            return new Texture(name, width, height);
        }

    }
}
