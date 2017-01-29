using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace KI.Foundation.ViewModel
{
    public class Matrix4ViewModel : MathViewModel
    {
        Func<Matrix4, bool> updateFunc;
        private Matrix4 Model;
        public Matrix4ViewModel(object owner,string name, Matrix4 value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<Matrix4, bool>(UpdateProperty);
        }

        public float M11
        {
            get { return Model.M11; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(value, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M12
        {
            get { return Model.M12; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, value, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M13
        {
            get { return Model.M13; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, value, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M14
        {
            get { return Model.M14; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, value, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M21
        {
            get { return Model.M21; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, value, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M22
        {
            get { return Model.M22; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, value, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M23
        {
            get { return Model.M23; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, value, M24, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M24
        {
            get { return Model.M24; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, value, M31, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M31
        {
            get { return Model.M31; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, value, M32, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M32
        {
            get { return Model.M32; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, value, M33, M34, M41, M42, M43, M44));
            }
        }
        public float M33
        {
            get { return Model.M33; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, value, M34, M41, M42, M43, M44));
            }
        }
        public float M34
        {
            get { return Model.M34; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, value, M41, M42, M43, M44));
            }
        }
        public float M41
        {
            get { return Model.M41; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, value, M42, M43, M44));
            }
        }
        public float M42
        {
            get { return Model.M42; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, value, M43, M44));
            }
        }
        public float M43
        {
            get { return Model.M43; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, value, M44));
            }
        }
        public float M44
        {
            get { return Model.M44; }
            set
            {
                SetValue<Matrix4>(updateFunc, new Matrix4(M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, value));
            }
        }

        public bool UpdateProperty(Matrix4 value)
        {
            Model = value;
            if (Owner == null)
                return false;

            var property = Owner.GetType().GetProperty(Name);
            if (property == null)
                return false;

            property.SetValue(Owner, value);

            return true;
        }

        public override void UpdateProperty()
        {

        }
    }
}
