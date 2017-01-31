using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using KI.Foundation.Core;
namespace KI.Gfx.KIAsset
{
    /// <summary>
    /// 光源
    /// </summary>
    public abstract class Light : KIObject
    {
        public Vector3 Position { get;private set; }
        public Vector3 Direction { get; private set; }
        public float Shiness { get; set; }
        public Matrix4 Matrix { get; private set; }

        private string _objectName = "Light";
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected Light(string name,Vector3 lightPos,Vector3 lightDir)
            :base(name)
        {
            Position = lightPos;
            Matrix = Matrix4.LookAt(Position, lightDir, Vector3.UnitY);
            Shiness = 1;
            if (name != null)
            {
                Name = name;
            }
            else
            {
                Name = _objectName;
            }
        }
        public override void Dispose()
        {

        }
    }
}
