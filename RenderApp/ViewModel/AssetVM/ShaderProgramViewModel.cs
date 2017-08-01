using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using KI.Gfx;
namespace RenderApp.ViewModel
{
    public class ShaderProgramViewModel : TabItemViewModel
    {
        public ShaderProgramViewModel(ShaderProgram model)
        {
            if (model != null)
            {
                Title = model.FileName;
            }

            Model = model;
        }

        public override string Title
        {
            get
            {
                if (title == null)
                {
                    return "ShaderProgram";
                }
                else
                {
                    return title;
                }
            }

            set
            {
                SetValue(ref title, value);
            }
        }

        private ShaderProgram model;
        public ShaderProgram Model
        {
            get
            {
                return model;
            }

            set
            {
                SetValue<ShaderProgram>(ref model, value);
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
