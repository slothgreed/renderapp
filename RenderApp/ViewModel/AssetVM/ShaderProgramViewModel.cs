using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.ViewModel.DockTabVM;
namespace RenderApp.ViewModel.AssetVM
{
    public class ShaderProgramViewModel : TabItemViewModel
    {

        public override  string Title
        {
            get
            {
                if(_title == null)
                {
                    return "ShaderProgram";
                }
                else
                {
                    return _title;
                }
            }
            set
            {
                SetValue(ref _title,value);
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
            if(model != null)
            {
                Title = model.FileName; 
            }
            Model = model;
        }



        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
