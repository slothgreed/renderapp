using KI.Gfx;
using KI.Gfx.KIShader;
using KI.Presenter.ViewModel;
using System.Collections.Generic;
using System.Windows.Input;

namespace ShaderTraining.ViewModel
{
    public class ShaderViewModel : ViewModelBase
    {
        private ICommand changeShaderCode;
        public ICommand ChangeShaderCode
        {
            get
            {
                if (changeShaderCode == null)
                {
                    changeShaderCode = CreateCommand(ChangeShaderCodeCommand);
                }

                return changeShaderCode;
            }

        }

        private Dictionary<string, Shader> shaders;

        public IEnumerable<string> ShaderNames
        {
            get
            {
                return shaders.Keys;
            }
        }

        private Shader selectShader;
        private string _selectShaderName;
        public string SelectShaderName
        {
            get
            {
                return _selectShaderName;
            }
            
            set
            {
                SetValue(ref _selectShaderName, value);
                selectShader = shaders[_selectShaderName];

                OnPropertyChanged(nameof(EnableVertex));
                OnPropertyChanged(nameof(EnableFrag));
                OnPropertyChanged(nameof(EnableGeom));
                OnPropertyChanged(nameof(EnableTCS));
                OnPropertyChanged(nameof(EnableTES));

                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.VertexShader);
            }
        }

        public bool EnableVertex
        {
            get
            {
                if (selectShader == null)
                {
                    return false;
                }


                return selectShader != null;
            }
        }

        public bool EnableFrag
        {
            get
            {
                if (selectShader == null)
                {
                    return false;
                }


                return selectShader.GetShaderProgram(ShaderKind.FragmentShader) != null;
            }
        }

        public bool EnableGeom
        {
            get
            {
                if (selectShader == null)
                {
                    return false;
                }


                return selectShader.GetShaderProgram(ShaderKind.GeometryShader) != null;
            }
        }

        public bool EnableTCS
        {
            get
            {
                if (selectShader == null)
                {
                    return false;
                }


                return selectShader.GetShaderProgram(ShaderKind.TessControlShader) != null;
            }
        }

        public bool EnableTES
        {
            get
            {
                if(selectShader == null)
                {
                    return false;   
                }

                return selectShader.GetShaderProgram(ShaderKind.TessEvaluationShader) != null;
            }
        }

        private ShaderProgram selectShaderProgram;
        public ShaderProgram SelectShaderProgram
        {
            get { return selectShaderProgram; }
            set
            {
                SetValue(ref selectShaderProgram, value);
            }
        }


        public ShaderViewModel(ViewModelBase parent, Dictionary<string, Shader> _shaders)
            : base(parent)
        {
            shaders = _shaders;
        }

        private void ChangeShaderCodeCommand(object parameter)
        {
            if(parameter is string == false)
            {
                return;
            }

            if(selectShader == null)
            {
                return;
            }

            string str = (string)parameter;
            if (str == "Vertex")
            {
                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.VertexShader);
            }
            else if (str == "Frag")
            {
                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.FragmentShader);
            }
            else if (str == "Geom")
            {
                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.GeometryShader);
            }
            else if (str == "TCS")
            {
                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.TessControlShader);
            }
            else if (str == "TES")
            {
                SelectShaderProgram = selectShader.GetShaderProgram(ShaderKind.TessEvaluationShader);
            }
            
        }

    }
}
