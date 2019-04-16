﻿using KI.Gfx;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// NURBS 曲面
    /// </summary>
    public class NURBSAttribute : AttributeBase
    {
        public int uOuter
        {
            get
            {
                return (int)Material.Shader.GetValue(nameof(uOuter));
            }

            set
            {
                Material.Shader.SetValue(nameof(uOuter), value);
            }
        }

        public int uInner
        {
            get
            {
                return (int)Material.Shader.GetValue(nameof(uInner));
            }

            set
            {
                Material.Shader.SetValue(nameof(uInner), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertexBuffer">線分</param>
        /// <param name="shader">シェーダ</param>
        public NURBSAttribute(string name, VertexBuffer vertexBuffer, Material material)
            : base(name, vertexBuffer, PolygonType.Patches, material)
        {
            uOuter = 2;
            uInner = 2;
        }
    }
}
