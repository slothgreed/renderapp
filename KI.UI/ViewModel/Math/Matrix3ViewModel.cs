using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.UI.ViewModel
{
    public class Matrix3ViewModel : PropertyViewModelBase
    {
        Func<Matrix3, bool> updateFunc;

        private Matrix3 Model;

        public Matrix3ViewModel(object owner, string name, Matrix3 value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<Matrix3, bool>(UpdateProperty);
        }

        public float M11
        {
            get { return Model.M11; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, M22, M23, M31, M32, M33));
            }
        }

        public float M12
        {
            get { return Model.M12; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, value, M13, M21, M22, M23, M31, M32, M33));
            }
        }

        public float M13
        {
            get { return Model.M13; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, value, M21, M22, M23, M31, M32, M33));
            }
        }

        public float M21
        {
            get { return Model.M21; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, value, M22, M23, M31, M32, M33));
            }
        }

        public float M22
        {
            get { return Model.M22; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, value, M23, M31, M32, M33));
            }
        }

        public float M23
        {
            get { return Model.M23; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, M22, value, M31, M32, M33));
            }
        }

        public float M31
        {
            get { return Model.M31; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, M22, M23, value, M32, M33));
            }
        }

        public float M32
        {
            get { return Model.M32; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, M22, M23, M31, value, M33));
            }
        }

        public float M33
        {
            get { return Model.M33; }
            set
            {
                SetValue<Matrix3>(updateFunc, new Matrix3(M11, M12, M13, M21, M22, M23, M31, M32, value));
            }
        }

        public bool UpdateProperty(Matrix3 value)
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
