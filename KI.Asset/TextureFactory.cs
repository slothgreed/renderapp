using System.Collections.Generic;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    public class TextureFactory : KIFactoryBase<Texture>
    {

        private static TextureFactory _instance = new TextureFactory();

        public static TextureFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public Texture CreateTexture(string path)
        {
            return CreateTexture(path, SelectImageKind(path));
        }

        public ImageKind SelectImageKind(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            extension = extension.ToLower();
            switch (extension)
            {
                case ".bmp":
                    return ImageKind.BMP;
                case ".png":
                    return ImageKind.PNG;
                case ".jpg":
                    return ImageKind.JPG;
                case ".tga":
                    return ImageKind.TGA;
                case ".hdr":
                    return ImageKind.HDR;
            }

            Logger.Log(Logger.LogLevel.Error, "not support texture");
            return ImageKind.None;
        }

        private KIImageInfo CreateImageInfo(string path, ImageKind kind)
        {
            switch (kind)
            {
                case ImageKind.PNG:
                case ImageKind.JPG:
                case ImageKind.BMP:
                    return new KIImageInfo(path);
                case ImageKind.TGA:
                    return new TGAImage(path);
                case ImageKind.HDR:
                    return new HDRImage(path);
                default:
                    Logger.Log(Logger.LogLevel.Error, "not support texture");
                    return null;
            }
        }

        public Texture CreateTexture(string path, ImageKind kind)
        {
            Texture find = FindByKey(path);
            if (find != null)
            {
                return find;
            }

            KIImageInfo image = CreateImageInfo(path, kind);
            Texture texture = new Texture(KIFile.GetNameFromPath(path), TextureType.Texture2D);
            AddItem(texture);
            texture.GenTexture(image);
            return texture;
        }

        public Texture CreateCubemapTexture(string px, string py, string pz, string nx, string ny, string nz)
        {
            List<KIImageInfo> images = new List<KIImageInfo>();

            images.Add(CreateImageInfo(px, SelectImageKind(px)));
            images.Add(CreateImageInfo(py, SelectImageKind(py)));
            images.Add(CreateImageInfo(pz, SelectImageKind(pz)));
            images.Add(CreateImageInfo(nx, SelectImageKind(nx)));
            images.Add(CreateImageInfo(ny, SelectImageKind(ny)));
            images.Add(CreateImageInfo(nz, SelectImageKind(nz)));
            Texture texture = new Texture("Cubemap" + KIFile.GetNameFromPath(px), TextureType.Cubemap);
            AddItem(texture);
            texture.GenCubemapTexture(images);
            return texture;
        }

        public Texture CreateTexture(string name, int width, int height)
        {
            Texture texture = new Texture(name, TextureType.Texture2D, width, height);
            AddItem(texture);
            return texture;
        }
    }
}
