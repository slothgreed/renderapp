using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.ViewModel
{
    public abstract class MathViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetValue<string>(ref _name, value);
            }
        }
    }


    public class Vector3ViewModel : MathViewModel
    {
        private Vector3 Model;
        public Vector3ViewModel(string name,Vector3 value)
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

    public class Vector2ViewModel : MathViewModel
    {
        private Vector2 Model;
        private string p;
        private Vector2ViewModel vector2ViewModel;
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
    public class Vector4ViewModel : MathViewModel
    {
        private Vector4 Model;
        public Vector4ViewModel(string name, Vector4 value)
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
        public float W{
            get { return Model.W; }
            set { SetValue<float>(ref Model.W, value); }
        }
        public override void UpdateProperty()
        {

        }
    }

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
    public class Matrix4ViewModel : MathViewModel
    {
        private Matrix4 Model;
        public Matrix4ViewModel(string name, Matrix4 value)
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
        public float M14
        {
            get { return Model.M14; }
            set { Model.M14 = value; OnPropertyChange(); }
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
        public float M24
        {
            get { return Model.M24; }
            set { Model.M24 = value; OnPropertyChange(); }
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
        public float M34
        {
            get { return Model.M34; }
            set { Model.M34 = value; OnPropertyChange(); }
        }
        public float M41
        {
            get { return Model.M41; }
            set { Model.M41 = value; OnPropertyChange(); }
        }
        public float M42
        {
            get { return Model.M42; }
            set { Model.M42 = value; OnPropertyChange(); }
        }
        public float M43
        {
            get { return Model.M43; }
            set { Model.M43 = value; OnPropertyChange(); }
        }
        public float M44
        {
            get { return Model.M44; }
            set { Model.M44 = value; OnPropertyChange(); }
        }
        public override void UpdateProperty()
        {

        }
    }
    public class NumericViewModel : MathViewModel
    {
        public float Model;
        public NumericViewModel(string name,float value)
        {
            this.Name = name;
            Model = value;
        }
        private float _value;
        public float Value
        {
            get { return Model; }
            set { SetValue<float>(ref Model, value); }
        }
        public override void UpdateProperty()
        {

        }
    }
}
