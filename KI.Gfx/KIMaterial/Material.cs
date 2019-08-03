using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;

namespace KI.Gfx
{
    /// <summary>
    /// マテリアル
    /// テクスチャ・シェーダ
    /// </summary>
    public class Material
    {
        /// <summary>
        /// マテリアル
        /// </summary>
        public Dictionary<TextureKind, Texture> Textures { get; private set; } = new Dictionary<TextureKind, Texture>();

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Material()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="material">シェーダ</param>
        /// <param name="shader">テクスチャ</param>
        public Material(Shader shader, Dictionary<TextureKind, Texture> textures)
        {
            Shader = shader;
            Textures = textures;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="material">シェーダ</param>
        public Material(Shader shader)
        {
            Shader = shader;
        }

        /// <summary>
        /// GPUにバインドする
        /// </summary>
        public virtual void BindToGPU()
        {
            Shader.BindBuffer();
        }

        /// <summary>
        /// GPUのバインドを解除する
        /// </summary>
        public virtual void UnBindToGPU()
        {
            Shader.UnBindBuffer();
        }


        /// <summary>
        /// テクスチャの追加
        /// </summary>
        /// <param name="kind">種類</param>
        /// <param name="texture">テクスチャ</param>
        public void AddTexture(TextureKind kind, Texture texture)
        {
            Textures[kind] = texture;
        }

        /// <summary>
        /// テクスチャのゲッタ
        /// </summary>
        /// <param name="kind">種類</param>
        /// <returns>テクスチャ</returns>
        public Texture GetTexture(TextureKind kind)
        {
            if (Textures.ContainsKey(kind))
            {
                return Textures[kind];
            }
            else
            {
                return null;
            }
        }
    }
}
