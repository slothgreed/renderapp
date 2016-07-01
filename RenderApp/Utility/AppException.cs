using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Utility
{
    class AppException : Exception
    {

        enum EAppError
        {

        }
        private Dictionary<EAppError, string> m_ErrorCode = new Dictionary<EAppError, string>();
        
        public AppException()
        {

        }
    }
}
