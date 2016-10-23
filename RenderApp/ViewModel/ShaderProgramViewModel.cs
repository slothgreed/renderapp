using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.ViewModel
{
    public class ShaderProgramViewModel : DockWindowViewModel
    {
        public override  string Title
        {
            get
            {
                return "Program";
            }
        }
                
        private ShaderProgram _model;
        public ShaderProgram Model
        {
            get
            {
                return _model;
            }
            set
            {
                SetValue<ShaderProgram>(ref _model, value);
            }
        }

        

        public ShaderProgramViewModel(ShaderProgram model)
        {
            Model = model;
        }



        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
