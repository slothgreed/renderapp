using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.ViewModel.MathVM
{
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
}
