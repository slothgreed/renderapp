using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.UI.ViewModel
{
    public class Vector4ViewModel : PropertyViewModelBase
    {
        Func<Vector4, bool> updateFunc;

        private Vector4 Model;

        public Vector4ViewModel(object owner, string name, Vector4 value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<Vector4, bool>(UpdateProperty);
        }

        public float X
        {
            get { return Model.X; }
            set
            {
                SetValue<Vector4>(updateFunc, new Vector4(value, Y, Z, W));
            }
        }

        public float Y
        {
            get { return Model.Y; }
            set
            {
                SetValue<Vector4>(updateFunc, new Vector4(X, value, Z, W));
            }
        }

        public float Z
        {
            get { return Model.Z; }
            set
            {
                SetValue<Vector4>(updateFunc, new Vector4(X, Y, value, W));
            }
        }

        public float W
        {
            get { return Model.W; }
            set
            {
                SetValue<Vector4>(updateFunc, new Vector4(X, Y, Z, value));
            }
        }

        public bool UpdateProperty(Vector4 value)
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
