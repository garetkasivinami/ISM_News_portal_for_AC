using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ISMNewsPortal.DAL_XML.Reflection
{
    public static class ReflectionParser
    {
        private static Dictionary<Type, PropertyInfo[]> propertyValueList = new Dictionary<Type, PropertyInfo[]>();

        public static List<PropertyValue> GetProperties<T>(T item)
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
        public static void SetPropertiesValues<T>(T item, List<PropertyValue> propertyValues)
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
                if (property != null)
                {
                    Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    object value = (string.IsNullOrEmpty(propertyValue.Value?.ToString())) ? null : Convert.ChangeType(propertyValue.Value, type);

                    property.SetValue(item, value);
                }
            }
        }
    }
}
