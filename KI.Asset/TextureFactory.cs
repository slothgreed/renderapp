using System.Collections.Generic;
using System.IO;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

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
            return CreateTexture(path, GetImageKind(path));
        }

        /// <summary>
        /// 画像種類の取得
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>画像種類</returns>
        public ImageKind GetImageKind(string path)
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
            Texture find = FindByName(path);
            if (find != null)
            {
                return find;
            }

            ImageInfo image = CreateImageInfo(path, kind);
            Texture texture = new Texture(Path.GetFileName(path), TextureType.Texture2D);
            texture.GenTexture(image);
            image.Dispose();

            return texture;
        }

        /// <summary>
        /// UVテクスチャの生成
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <returns>テクスチャ</returns>
        public Texture CreateUVTexture(int size)
        {
            Texture texture = new Texture("UV Texture", TextureType.Texture2D);
            float[,,] rgba = new float[size, size, 4];
            float halfSize = size / 2;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (halfSize < i && halfSize < j ||
                        halfSize > i && halfSize > j)
                    {
                        rgba[i, j, 0] = 255;
                        rgba[i, j, 1] = 255;
                        rgba[i, j, 2] = 255;
                        rgba[i, j, 3] = 255;
                    }
                    else
                    {
                        rgba[i, j, 0] = 0;
                        rgba[i, j, 1] = 0;
                        rgba[i, j, 2] = 0;
                        rgba[i, j, 3] = 255;
                    }
                }
            }

            texture.GenTexture(rgba);
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
            var texture = new Texture(name, TextureType.Texture2D, width, height);
            return texture;
        }

        public RenderTexture CreateRenderTexture(string name, int width, int height)
        {
            var renderTexture = new RenderTexture(name, width, height);
            return renderTexture;
        }

        /// <summary>
        /// キューブマップの作成(実際にCubeMapをレンダリングしていると同じテクスチャを2つ持つ状態になっている)
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
            List<ImageInfo> images = new List<ImageInfo>();
            images.Add(CreateImageInfo(px, GetImageKind(px)));
            images.Add(CreateImageInfo(py, GetImageKind(py)));
            images.Add(CreateImageInfo(pz, GetImageKind(pz)));
            images.Add(CreateImageInfo(nx, GetImageKind(nx)));
            images.Add(CreateImageInfo(ny, GetImageKind(ny)));
            images.Add(CreateImageInfo(nz, GetImageKind(nz)));
            Texture texture = new Texture("Cubemap" + Path.GetFileName(px), TextureType.Cubemap);
            texture.GenCubemapTexture(images);

            return texture;
        }

        /// <summary>
        /// 画像情報の作成
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="kind">画像の種類</param>
        /// <returns>画像</returns>
        private ImageInfo CreateImageInfo(string path, ImageKind kind)
        {
            ImageInfo image;
            switch (kind)
            {
                case ImageKind.PNG:
                case ImageKind.JPG:
                case ImageKind.BMP:
                    image = new ImageInfo(path);
                    break;
                case ImageKind.TGA:
                    image = new TGAImage(path);
                    break;
                case ImageKind.HDR:
                    image = new HDRImage(path);
                    break;
                default:
                    Logger.Log(Logger.LogLevel.Error, "not support texture");
                    return null;
            }

            image.Load(path);
            return image;
        }
    }
}
