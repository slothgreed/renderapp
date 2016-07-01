using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Assets;
using RenderApp.GLUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using RenderApp.Analyzer;
namespace RenderApp.Assets
{
    public class Material : Asset
    {

        #region [property method]
        public static string _name = "Material";
        private Shader _shader = null;
        public Shader ShaderItem
        {
            get
            {
                return _shader;
            }
            private set
            {
                _shader = value;
                _shader.AnalizeShaderProgram();
            }
        }
        public Dictionary<TextureKind,Texture> TextureItem
        {
            get;
            set;
        }
        public Dictionary<string, Texture> FrameBufferItem
        {
            get;
            set;
        }
        public List<IAnalyzer> AnalyzeItem
        {
            get;
            set;
        }
        #endregion
        #region [constructor]
        private void Initialize(string name = null, Shader shader = null)
        {
            if (shader == null)
            {
                SetShader(Scene.DefaultShader);
            }
            else
            {
                SetShader(shader);
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
        #region [setter]
        public void SetShader(Shader shader)
        {
            ShaderItem = shader;
        }
        public Material(string name)
            : base(name)
        {
            Initialize(name);
        }
        public Material(Shader shader)
        {
            Initialize(null, shader);
        }
        #endregion
        #region [texture bind]
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
        #region analyzer bind
        public void AddAnalayzer(IAnalyzer analyze)
        {
            if(AnalyzeItem == null)
            {
                AnalyzeItem = new List<IAnalyzer>();
            }
            AnalyzeItem.Add(analyze);
        }
        #endregion
        #region [bind shader]
        /// <summary>
        /// シェーダにBinding
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public bool BindShader(Geometry geometry)
        {
            ShaderItem.BindBuffer(geometry);
            return true;
        }

        public bool UnBindShader()
        {
            ShaderItem.UnBindBuffer();
            return true;
        }
        #endregion

        public override void Dispose()
        {
            ShaderItem.Dispose();
        }

        public override string Key
        {
            get;
            set;
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

        internal void InitializeState(Geometry geometry)
        {
            ShaderItem.InitializeState(geometry, TextureItem);
        }
    }
}
