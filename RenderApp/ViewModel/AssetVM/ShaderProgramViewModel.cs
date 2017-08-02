using System;
using KI.Gfx;

namespace RenderApp.ViewModel
{
    public class ShaderProgramViewModel : TabItemViewModel
    {
        private ShaderProgram model;

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
