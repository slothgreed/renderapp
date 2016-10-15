using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;
namespace RenderApp.ViewModel
{
    public class PropertyGridViewModel : DynamicObject,INotifyPropertyChanged//, ICustomTypeDescriptor
    {
        private readonly Dictionary<string, object> dynamicProperties = new Dictionary<string, object>();
        public PropertyGridViewModel(Dictionary<string,object> model)
        {
            foreach (KeyValuePair<string, object> loop in model)
            {
                if (loop.Value is GLUtil.Texture)
                {
                    dynamicProperties.Add(loop.Key, new ImageViewModel(loop.Key,(GLUtil.Texture)loop.Value));
                }
                else if (loop.Value is OpenTK.Vector2)
                {
                    dynamicProperties.Add(loop.Key + "X", ((OpenTK.Vector2)loop.Value).X);
                    dynamicProperties.Add(loop.Key + "Y", ((OpenTK.Vector2)loop.Value).Y);
                }
                else if (loop.Value is OpenTK.Vector3)
                {
                    dynamicProperties.Add(loop.Key + "X", ((OpenTK.Vector3)loop.Value).X);
                    dynamicProperties.Add(loop.Key + "Y", ((OpenTK.Vector3)loop.Value).Y);
                    dynamicProperties.Add(loop.Key + "Z", ((OpenTK.Vector3)loop.Value).Z);
                }
                else if (loop.Value is OpenTK.Vector4)
                {
                    dynamicProperties.Add(loop.Key + "X", ((OpenTK.Vector4)loop.Value).X);
                    dynamicProperties.Add(loop.Key + "Y", ((OpenTK.Vector4)loop.Value).Y);
                    dynamicProperties.Add(loop.Key + "Z", ((OpenTK.Vector4)loop.Value).Z);
                    dynamicProperties.Add(loop.Key + "W", ((OpenTK.Vector4)loop.Value).W);

                }
                else if (loop.Value is OpenTK.Matrix3)
                {
                    dynamicProperties.Add(loop.Key, new Matrix3ViewModel(loop.Key, (OpenTK.Matrix3)loop.Value));

                }
                else if (loop.Value is OpenTK.Matrix4)
                {
                    dynamicProperties.Add(loop.Key, new Matrix4ViewModel(loop.Key, (OpenTK.Matrix4)loop.Value));

                }
                else if (loop.Value is NumericViewModel)
                {
                    dynamicProperties.Add(loop.Key, new NumericViewModel(loop.Key, (float)loop.Value));
                }
                else
                {
                    dynamicProperties.Add(loop.Key, new DefaultViewModel(loop.Key,loop.Value.ToString()));
                }
            }
        }


        #region [dynamic object]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var memberName = binder.Name;
            if (dynamicProperties.ContainsKey(memberName))
            {
                result = dynamicProperties[memberName];
                return true;
            }
            result = null;
            return false;

        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;
            if (!dynamicProperties.ContainsKey(memberName))
            {
                dynamicProperties.Add(memberName, value);
            }
            else
            {
                dynamicProperties[memberName] = value;
            }
            return true;
        }

        //public PropertyDescriptorCollection GetProperties()
        //{
        //    var attributes = new Attribute[0];
        //    var properties = dynamicProperties.Select(pair => new DynamicPropertyDescriptor(this,
        //        pair.Key, pair.Value.GetType(), attributes));
        //    return new PropertyDescriptorCollection(properties.ToArray());
        //}

        public string GetClassName()
        {
            return GetType().Name;
        }
        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            var eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, eventArgs);
        }

        private void NotifyToRefreshAllProperties()
        {
            OnPropertyChanged(String.Empty);
        }

        #endregion

        //#region [DynamicPropertyDescriptor]
        //private class DynamicPropertyDescriptor : PropertyDescriptor
        //{
        //    private readonly PropertyGridViewModel propertyObject;
        //    private readonly Type propertyType;

        //    public DynamicPropertyDescriptor(PropertyGridViewModel propertyObject, string propertyName, Type propertyType, Attribute[] propertyAttributes)
        //        : base(propertyName, propertyAttributes)
        //    {
        //        this.propertyObject = propertyObject;
        //        this.propertyType = propertyType;
        //    }

        //    public override bool CanResetValue(object component)
        //    {
        //        return true;
        //    }
        //    public override object GetValue(object component)
        //    {
        //        return propertyObject.dynamicProperties[Name];
        //    }
        //    public override void ResetValue(object component)
        //    {
        //    }
        //    public override void SetValue(object component, object value)
        //    {
        //        propertyObject.dynamicProperties[Name] = value;
        //    }
        //    public override bool ShouldSerializeValue(object component)
        //    {
        //        return false;
        //    }
        //    public override Type ComponentType
        //    {
        //        get { return typeof(PropertyGridViewModel); }
        //    }
        //    public override bool IsReadOnly
        //    {
        //        get { return false; }
        //    }
        //    public override Type PropertyType
        //    {
        //        get { return propertyType; }
        //    }
        //}
        //#endregion

        //#region [ICustomTypeDescriptor]
        //AttributeCollection ICustomTypeDescriptor.GetAttributes()
        //{
        //    return new AttributeCollection(null);
        //}
        //string ICustomTypeDescriptor.GetClassName()
        //{
        //    return null;
        //}
        //string ICustomTypeDescriptor.GetComponentName()
        //{
        //    return null;
        //}
        //TypeConverter ICustomTypeDescriptor.GetConverter()
        //{
        //    return null;
        //}
        //EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        //{
        //    return null;
        //}
        //PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        //{
        //    return null;
        //}
        //object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        //{
        //    return null;
        //}
        //EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        //{
        //    return new EventDescriptorCollection(null);
        //}
        //EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        //{
        //    return new EventDescriptorCollection(null);
        //}
        //PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        //{
        //    var properties = dynamicProperties.Select(pair => new DynamicPropertyDescriptor(this,
        //        pair.Key, pair.Value.GetType(), attributes));
        //    return new PropertyDescriptorCollection(properties.ToArray());
        //}
        //PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        //{
        //    return ((ICustomTypeDescriptor)this).GetProperties(null);
        //}
        //object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        //{
        //    return this;
        //}
        //#endregion



    }
}
