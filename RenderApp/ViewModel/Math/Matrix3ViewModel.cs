using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.ViewModel.MathVM
{
    public class Matrix3ViewModel : MathViewModel
    {
        private Matrix3 Model;
        public Matrix3ViewModel(string name, Matrix3 value)
        {
            Name = name;
            Model = value;
        }

        public float M11
        {
            get { return Model.M11; }
            set { Model.M11 = value; OnPropertyChange(); }
        }
        public float M12
        {
            get { return Model.M12; }
            set { Model.M12 = value; OnPropertyChange(); }
        }
        public float M13
        {
            get { return Model.M13; }
            set { Model.M13 = value; OnPropertyChange(); }
        }
        public float M21
        {
            get { return Model.M21; }
            set { Model.M21 = value; OnPropertyChange(); }
        }
        public float M22
        {
            get { return Model.M22; }
            set { Model.M22 = value; OnPropertyChange(); }
        }
        public float M23
        {
            get { return Model.M23; }
            set { Model.M23 = value; OnPropertyChange(); }
        }
        public float M31
        {
            get { return Model.M31; }
            set { Model.M31 = value; OnPropertyChange(); }
        }
        public float M32
        {
            get { return Model.M32; }
            set { Model.M32 = value; OnPropertyChange(); }
        }
        public float M33
        {
            get { return Model.M33; }
            set { Model.M33 = value; OnPropertyChange(); }
        }
        public override void UpdateProperty()
        {

        }
    }
}
