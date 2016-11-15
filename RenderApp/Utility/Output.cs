using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using System.Diagnostics;
namespace RenderApp.Utility
{
    class Output
    {
        public static void Error(string error)
        {
            Debug.WriteLine(error);
        }
        public static void GLError([CallerMemberName]string methodName = "")
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine(methodName + ":" + error);
            }
        }
    }
}
