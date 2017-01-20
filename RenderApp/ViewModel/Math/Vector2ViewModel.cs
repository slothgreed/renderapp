using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.ViewModel.MathVM
{
    public class Vector2ViewModel : MathViewModel
    {
        private Vector2 Model;

        public Vector2ViewModel(string name, Vector2 value)
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
        public override void UpdateProperty()
        {

        }
    }
}
