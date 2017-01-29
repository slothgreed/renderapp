using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STLBrowser.Model
{
    public class STLInfo
    {
        public int VertexNum
        {
            get;
            private set;
        }
        public int EdgeNum
        {
            get;
            private set;
        }
        public int FaceNum
        {
            get;
            private set;
        }

        public STLInfo(string path)
        {
            VertexNum = 100;
            EdgeNum = 100;
            FaceNum = 100;
        }
    }
}
