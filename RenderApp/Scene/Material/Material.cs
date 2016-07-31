using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Analyzer;
using RenderApp.AssetModel.ShaderModel;
namespace RenderApp.AssetModel.MaterialModel
{
    public class Material : Asset
    {

        #region [property method]
        public static string _name = "Material";
        #endregion

        #region [shader]
        /// <summary>
        /// rendermodeの固定
        /// </summary>
        public ERenderMode Only
        {
            get;
            private set;
        }
        private Shader _shader = null;
        public Shader CurrentShader
        {
            get;
            private set;
        }
        private Shader _forward;
        public Shader Forward
        {
            get
            {
                return _forward;
            }
            private set
            {
                _forward = value;
                _forward.AnalizeShaderProgram();
            }
        }
        private Shader _deffered;
        public Shader Defferd
        {
            get
            {
                return _deffered;
            }
            private set
            {
                _deffered = value;
                _deffered.AnalizeShaderProgram();
            }
        }
        public void SetShader(Shader shader)
        {
            if (shader.RenderMode == ERenderMode.Forward)
            {
                Forward = shader;
            }
            else if (shader.RenderMode == ERenderMode.Deffered)
            {
                Defferd = shader;
            }
        }
        internal void ChangeRenderMode(ERenderMode mode)
        {
            if (mode == ERenderMode.Deffered)
            {
                CurrentShader = Defferd;
            }
            else if (mode == ERenderMode.Forward)
            {
                CurrentShader = Forward;
            }
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
                SetShader(ShaderFactory.Instance.DefaultForwardShader);
                SetShader(ShaderFactory.Instance.DefaultDefferedShader);
                CurrentShader = Forward;
            }

            TextureItem = new Dictionary<TextureKind, Texture>();
            AnalyzeItem = new List<IAnalyzer>();
            if (name == null)
            {
                Key = _name;
            }
            Scene.ActiveScene.AddSceneObject(_name, this);
        }
        #endregion
        #region [texture]

        public Dictionary<TextureKind, Texture> TextureItem
        {
            get;
            set;
        }
        public Dictionary<string, Texture> FrameBufferItem
        {
            get;
            set;
        }
        public void AddTexture(TextureKind kind,Texture texture)
        {
            TextureItem[kind] = texture;
        }
        public void AddTexture(string fullpath, string filename, TextureKind kind)
        {
            Texture texture = Scene.ActiveScene.FindObject(filename, EAssetType.Textures) as Texture;
            if (texture == null)
            {
                texture = new Texture(fullpath);
            }
            if(TextureItem.ContainsKey(kind))
            {
                TextureItem[kind] = texture;
            }
            else
            {
                TextureItem.Add(kind, texture);
            }
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
        public List<IAnalyzer> AnalyzeItem
        {
            get;
            set;
        }
        public void AddAnalayzer(IAnalyzer analyze)
        {
            if(AnalyzeItem == null)
            {
                AnalyzeItem = new List<IAnalyzer>();
            }
            AnalyzeItem.Add(analyze);
        }
        #endregion

        public override void Dispose()
        {
            CurrentShader = null;
            if(Forward != null)
            {
                Forward.Dispose();
            }
            if (Defferd != null)
            {
                Defferd.Dispose();
            }
        }

        public static Material _default;
        public static Material Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new Material("Default");
                }
                return _default;
            }
        }



    }
}
