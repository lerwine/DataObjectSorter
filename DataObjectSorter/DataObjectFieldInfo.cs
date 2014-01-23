using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace DataObjectSorter
{
    public class DataObjectFieldInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public DataObjectFieldInfo(PropertyInfo property, BindableAttribute bindableAttribute)
        {
            this.PropertyInfo = property;
            this.IsReadOnly = DataObjectFieldInfo.GetLastAttributeValue<ReadOnlyAttribute, bool>(property, 
                (ReadOnlyAttribute a) => a != null && a.IsReadOnly);
            this.Direction = (property.CanRead && property.CanWrite) ? ((bindableAttribute == null && !this.IsReadOnly) ? BindingDirection.TwoWay :  bindableAttribute.Direction) :
                BindingDirection.OneWay;
            this.DisplayName = DataObjectFieldInfo.GetLastAttributeValue<DisplayNameAttribute, string>(property, (DisplayNameAttribute a) => (a == null) ? property.Name : a.DisplayName, (DisplayNameAttribute a) => !String.IsNullOrWhiteSpace(a.DisplayName));
            Type pt = property.PropertyType;
            Type ct = typeof(IComparable<>);
            if (pt.IsGenericType && pt.
        }
        public static A GetLastAttribute<A>(MemberInfo memberInfo)
            where A : Attribute
        {
            if (memberInfo == null)
                return null;

            return memberInfo.GetCustomAttributes(typeof(A), true).OfType<A>().LastOrDefault(a => a != null);
        }

        public static A GetLastAttribute<A>(MemberInfo memberInfo, Func<A, bool> validation)
            where A : Attribute
        {
            if (memberInfo == null)
                return null;

            return memberInfo.GetCustomAttributes(typeof(A), true).OfType<A>().LastOrDefault(a => a != null && validation(a));
        }

        public static T GetLastAttributeValue<A, T>(MemberInfo memberInfo, Func<A, T> getValue)
            where A : Attribute
        {
            if (memberInfo == null)
                return getValue(null);

            return getValue(memberInfo.GetCustomAttributes(typeof(A), true).OfType<A>().LastOrDefault(a => a != null));
        }

        public static T GetLastAttributeValue<A, T>(MemberInfo memberInfo, Func<A, T> getValue, Func<A, bool> validation)
            where A : Attribute
        {
            if (memberInfo == null)
                return getValue(null);

            return getValue(memberInfo.GetCustomAttributes(typeof(A), true).OfType<A>().LastOrDefault(a => a != null && validation(a)));
        }

        internal static DataObjectFieldInfo Create(PropertyInfo property)
        {
            BindableAttribute bindableAttribute;
            if (property == null || ((bindableAttribute = DataObjectFieldInfo.GetLastAttribute<BindableAttribute>(property)) != null && !bindableAttribute.Bindable))
                return null;

            return new DataObjectFieldInfo(property, bindableAttribute);
        }

        public BindingDirection Direction { get; set; }

        public bool IsReadOnly { get; set; }

        public string DisplayName { get; set; }
    }
}
