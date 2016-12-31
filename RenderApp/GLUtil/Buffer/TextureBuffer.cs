using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.GLUtil.Buffer
{
    public class TextureBuffer : BufferObject
    {
        public TextureTarget Target
        {
            get;
            set;
        }
        public TextureBuffer()
        {
            Target = TextureTarget.Texture2D;
        }
        public TextureBuffer(TextureTarget target)
        {
            Target = target;
        }
        public override void GenBuffer()
        {
            ID = GL.GenTexture();
        }
        public void SetData()
        {

        }
        public override void BindBuffer()
        {
            GL.BindTexture(Target, ID);
        }
        public override void UnBindBuffer()
        {
            GL.BindTexture(Target, 0);
        }
        public override void Dispose()
        {
            GL.DeleteTexture(ID);
        }
    }
}
