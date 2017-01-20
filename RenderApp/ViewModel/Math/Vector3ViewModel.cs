using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.ViewModel.MathVM
{
    public class Vector3ViewModel : MathViewModel
    {
        private Vector3 Model;
        public Vector3ViewModel(string name, Vector3 value)
        {
            Name = name;
            Model = value;
        }
        public float X
        {
            get { return Model.X; }
            set { SetValue<float>(ref Model.X, value); }
        }
        public float Y
        {
            get { return Model.Y; }
            set { SetValue<float>(ref Model.Y, value); }
        }
        public float Z
        {
            get { return Model.Z; }
            set { SetValue<float>(ref Model.Z, value); }
        }
        public override void UpdateProperty()
        {

        }
    }
}
