using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ISMNEWSPORTAL.DAL_XML
{
    public class ReflectionParse
    {
        private static Dictionary<Type, PropertyInfo[]> propertyValueList = new Dictionary<Type, PropertyInfo[]>();

        public List<PropertyValue> GetProperties<T>(T item)
        {
            Type t = typeof(T);
            PropertyInfo[] properties;

            if (propertyValueList.ContainsKey(t))
                properties = propertyValueList[t];
            else
                properties = t.GetProperties();

            var values = new List<PropertyValue>();
            foreach(PropertyInfo propertyInfo in properties)
            {
                values.Add(new PropertyValue(propertyInfo, item));
            }
            return values;
        }
        public void SetPropertiesValues<T>(T item, List<PropertyValue> propertyValues)
        {
            Type t = typeof(T);
            PropertyInfo[] properties;

            if (propertyValueList.ContainsKey(t))
                properties = propertyValueList[t];
            else
                properties = t.GetProperties();

            var values = new List<PropertyValue>();
            foreach (PropertyValue propertyValue in propertyValues)
            {
                PropertyInfo property = properties.SingleOrDefault(u => u.Name == propertyValue.Name);
                Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                object value = (string.IsNullOrEmpty(propertyValue.Value?.ToString())) ? null : Convert.ChangeType(propertyValue.Value, type);

                property.SetValue(item, value);
            }
        }
    }
    
    public class PropertyValue
    {
        public string Name;
        public object Value;
        public PropertyValue(PropertyInfo propertyInfo, object target)
        {
            Name = propertyInfo.Name;
            Value = propertyInfo.GetValue(target);
        }
        public PropertyValue(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public void SetValue(object value)
        {
            Value = value;
        }
    }
}
