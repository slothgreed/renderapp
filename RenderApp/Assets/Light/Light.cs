using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using RenderApp.Utility;
using RenderApp.Assets;
namespace RenderApp.Assets
{
    /// <summary>
    /// 光源
    /// </summary>
    public abstract class Light : Asset
    {
        public Vector3 Position { get;private set; }
        public Vector3 Direction { get; private set; }
        public float Shiness { get; set; }
        public Matrix4 Matrix { get; private set; }

        private string _objectName = "Light";
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected Light(Vector3 lightPos,Vector3 lightDir,string name = null)
        {
            Position = lightPos;
            Matrix = Matrix4.LookAt(Position, lightDir, Vector3.UnitY);
            Shiness = 1;
            if (name != null)
            {
                Key = name;
            }
            else
            {
                Key = _objectName;
            }
            Scene.ActiveScene.AddSceneObject(Key, this);
        }
        public override void Dispose()
        {

        }
    }
}
