using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Dynamic;
namespace RenderApp.ViewModel
{
    public class DynamicViewModelBase<T> : DynamicObject, INotifyPropertyChanged where T : class
    {
        private T _model;
        protected T Model
        {
            get
            {
                return _model;
            }
        }

        public DynamicViewModelBase(T model)
        {
            this._model = model;
        }

        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            var h = PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(name));
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;
            var property = _model.GetType().GetProperty(propertyName);
            if(property == null || !property.CanRead)
            {
                result = null;
                return false;
            }
            result = property.GetValue(_model, null);
            
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyName = binder.Name;
            var property = _model.GetType().GetProperty(propertyName);
            if (property == null || !property.CanWrite)
            {
                return false;
            }
            property.SetValue(_model, value, null);
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
