using System.ComponentModel;
using System.Dynamic;
namespace RenderApp.ViewModel
{
    public class DynamicViewModelBase<T> : DynamicObject, INotifyPropertyChanged where T : class
    {
        private T model;
        protected T Model
        {
            get
            {
                return model;
            }
        }

        public DynamicViewModelBase(T model)
        {
            this.model = model;
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
            var property = model.GetType().GetProperty(propertyName);
            if (property == null || !property.CanRead)
            {
                result = null;
                return false;
            }

            result = property.GetValue(model, null);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyName = binder.Name;
            var property = model.GetType().GetProperty(propertyName);
            if (property == null || !property.CanWrite)
            {
                return false;
            }

            property.SetValue(model, value, null);
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
