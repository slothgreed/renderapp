using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.UI.ViewModel
{
    public class Vector3ViewModel : PropertyViewModelBase
    {
        Func<Vector3, bool> updateFunc;

        private Vector3 Model;

        public Vector3ViewModel(object owner, string name, Vector3 value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<Vector3, bool>(UpdateProperty);
        }

        public float X
        {
            get { return Model.X; }
            set
            {
                SetValue<Vector3>(updateFunc, new Vector3(value, Y, Z));
            }
        }

        public float Y
        {
            get { return Model.Y; }
            set
            {
                SetValue<Vector3>(updateFunc, new Vector3(X, value, Z));
            }
        }

        public float Z
        {
            get { return Model.Z; }
            set
            {
                SetValue<Vector3>(updateFunc, new Vector3(X, Y, value));
            }
        }

        public bool UpdateProperty(Vector3 value)
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
            throw new NotImplementedException();
        }
    }
}
