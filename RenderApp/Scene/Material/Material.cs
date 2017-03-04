using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Core;
using RenderApp.GLUtil.ShaderModel;
using KI.Gfx.KIAsset;
namespace RenderApp.AssetModel
{
    public class Material : KIFile
    {

        #region [property method]
        public static string _name = "Material";
        #endregion

        #region [shader]
        public Shader CurrentShader
        {
            get;
            set;
        }
        #endregion

        #region [constructor]
        public Material(string name)
            : base(name)
        {
            Initialize(name);
        }
        private void Initialize(string name = null, Shader shader = null)
        {
            if (shader == null)
            {
                CurrentShader = (ShaderFactory.Instance.DefaultDefferredShader);
            }
        }
        #endregion
        #region [texture]
        #endregion
        public override void Dispose()
        {

        }
        /// <summary>
        /// Tempテクスチャを設定するマテリアル
        /// </summary>
        public static Material _default;
        public static Material Default
        {
            get
            {
                if(_default == null)
                {
                    _default = AssetFactory.Instance.CreateMaterial("Default");
                    _default.CurrentShader = (ShaderFactory.Instance.DefaultDefferredShader);
                }
                return _default;
            }
        }
        /// <summary>
        /// テクスチャなしマテリアル
        /// </summary>
        private static Material _constant;
        public static Material Constant
        {
            get
            {
                if(_constant == null)
                {
                    _constant = AssetFactory.Instance.CreateMaterial("Analyze");
                    _constant.CurrentShader = (ShaderFactory.Instance.DefaultAnalyzeShader);
                }
                return _constant;
            }
        }


    }
}
