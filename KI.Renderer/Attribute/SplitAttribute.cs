using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Attribute
{
    /// <summary>
    /// ディスプレイスメントマップ
    /// </summary>
    public class SplitAttribute : AttributeBase
    {
        public int uOuter
        {
            get
            {
                return (int)Shader.GetValue(nameof(uOuter));
            }

            set
            {
                Shader.SetValue(nameof(uOuter), value);
            }
        }

        public int uInner
        {
            get
            {
                return (int)Shader.GetValue(nameof(uInner));
            }

            set
            {
                Shader.SetValue(nameof(uInner), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertexBuffer">線分</param>
        /// <param name="shader">シェーダ</param>
        public SplitAttribute(string name, VertexBuffer vertexBuffer, Shader shader)
            : base(name, vertexBuffer, PrimitiveType.Patches, shader)
        {
            uOuter = 2;
            uInner = 2;
        }
    }
}
