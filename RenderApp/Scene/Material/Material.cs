﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Analyzer;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.AssetModel
{
    public class Material : Asset
    {

        #region [property method]
        public static string _name = "Material";
        #endregion

        #region [shader]
        private Shader _currentShader = null;
        public Shader CurrentShader
        {
            get
            {
                return _currentShader;
            }
            private set
            {
                _currentShader = value;
            }
        }
        public void SetShader(Shader shader)
        {
            CurrentShader = shader;
        }
        internal void InitializeState(Geometry geometry)
        {
            CurrentShader.InitializeState(geometry, TextureItem);
        }
        /// <summary>
        /// シェーダにBinding
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public bool BindShader(Geometry geometry)
        {
            CurrentShader.BindBuffer(geometry);
            return true;
        }

        public bool UnBindShader()
        {
            CurrentShader.UnBindBuffer();
            return true;
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
                SetShader(ShaderFactory.Instance.DefaultDefferredShader);
            }

            TextureItem = new Dictionary<TextureKind, Texture>();
            AnalyzeItem = new Dictionary<string,IAnalyzer>();
            if (name == null)
            {
                Key = _name;
            }
        }
        #endregion
        #region [texture]

        public Dictionary<TextureKind, Texture> TextureItem
        {
            get;
            private set;
        }
        public Dictionary<string, Texture> FrameBufferItem
        {
            get;
            private set;
        }
        public void AddTexture(TextureKind kind,Texture texture)
        {
            TextureItem[kind] = texture;
        }
        public int TextureNum()
        {
            if(TextureItem == null)
            {
                return 0;
            }
            return TextureItem.Count;
        }
        public Texture GetTexture(TextureKind kind)
        {
            if(TextureItem.ContainsKey(kind))
            {
                return TextureItem[kind];
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<Texture> GetTexture()
        {
            foreach(var loop in TextureItem.Keys)
            {
                yield return TextureItem[loop];
            }
        }
        #endregion
        #region [analyzer bind]
        private Dictionary<string,IAnalyzer> AnalyzeItem
        {
            get;
            set;
        }
        public void AddAnalayzer(IAnalyzer analyze)
        {
            if (AnalyzeItem == null)
            {
                AnalyzeItem = new Dictionary<string,IAnalyzer>();
            }
            AnalyzeItem.Add(analyze.GetType().Name, analyze);
        }
        public IAnalyzer FindAnalyze(string typeName)
        {
            if(AnalyzeItem.ContainsKey(typeName))
            {
                return AnalyzeItem[typeName];
            }
            return null;
        }
        #endregion

        public override void Dispose()
        {

        }

        public static Material _default;
        public static Material Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new Material("NoMaterial");
                }
                return _default;
            }
        }



    }
}
