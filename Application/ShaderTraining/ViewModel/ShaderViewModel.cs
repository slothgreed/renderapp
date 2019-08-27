using KI.Gfx;
using KI.Gfx.KIShader;
using KI.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                SelectShaderProgram = selectShader.VertexShader;
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


                return selectShader.FragShader != null;
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


                return selectShader.GeomShader != null;
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


                return selectShader.TcsShader != null;
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

                return selectShader.TesShader != null;
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
                SelectShaderProgram = selectShader.VertexShader;
            }
            else if (str == "Frag")
            {
                SelectShaderProgram = selectShader.FragShader;
            }
            else if (str == "Geom")
            {
                SelectShaderProgram = selectShader.GeomShader;
            }
            else if (str == "TCS")
            {
                SelectShaderProgram = selectShader.TcsShader;
            }
            else if (str == "TES")
            {
                SelectShaderProgram = selectShader.TesShader;
            }
            
        }

    }
}
