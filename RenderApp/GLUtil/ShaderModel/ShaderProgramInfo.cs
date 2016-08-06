using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
namespace RenderApp.GLUtil.ShaderModel
{

    /// <summary>
    /// シェーダ情報
    /// </summary>
    public class ShaderProgramInfo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int VertexBufferId { get; set; }
        public object variable { get; set; }
        public EVariableType variableType { get; set; }
    }
}
