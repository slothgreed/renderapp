using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    /// <summary>
    /// テクスチャファクトリ
    /// </summary>
    public class TextureFactory : KIFactoryBase<Texture>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static TextureFactory Instance { get; } = new TextureFactory();

        /// <summary>
        /// テクスチャの作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>テクスチャ</returns>
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

        /// <summary>
        /// テクスチャの作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="kind">テクスチャ種類</param>
        /// <returns>テクスチャ</returns>
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

        /// <summary>
        /// テクスチャの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <returns>テクスチャ</returns>
        public Texture CreateTexture(string name, int width, int height)
        {
            Texture texture = new Texture(name, TextureType.Texture2D, width, height);
            AddItem(texture);
            return texture;
        }

        /// <summary>
        /// キューブマップの作成
        /// </summary>
        /// <param name="px">PXファイルパス</param>
        /// <param name="py">PYファイルパス</param>
        /// <param name="pz">PZファイルパス</param>
        /// <param name="nx">NXファイルパス</param>
        /// <param name="ny">NYファイルパス</param>
        /// <param name="nz">NZファイルパス</param>
        /// <returns>テクスチャ</returns>
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

        /// <summary>
        /// 画像情報の作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="kind">画像の種類</param>
        /// <returns>画像</returns>
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
    }
}
