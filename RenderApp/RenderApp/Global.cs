using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp
{
    public static class Global
    {
        public static string SphereMapAlbedo = Project.TextureDirectory + @"\SphreMap.jpg";
        public static string SphereMapVertexShader = Project.ShaderDirectory + @"\sphereMap.vert";
        public static string SphereMapFragmentShader = Project.ShaderDirectory + @"\sphereMap.frag";
    }
}
