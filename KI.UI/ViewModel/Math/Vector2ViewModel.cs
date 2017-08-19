using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.UI.ViewModel
{
    public class Vector2ViewModel : PropertyViewModelBase
    {
        Func<Vector2, bool> updateFunc;

        private Vector2 Model;

        public Vector2ViewModel(object owner, string name, Vector2 value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<Vector2, bool>(UpdateProperty);
        }

        public float X
        {
            get { return Model.X; }
            set
            {
                SetValue<Vector2>(updateFunc, new Vector2(value, Y));
            }
        }

        public float Y
        {
            get { return Model.Y; }
            set
            {
                SetValue<Vector2>(updateFunc, new Vector2(value, Y));
            }
        }

        public bool UpdateProperty(Vector2 value)
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
