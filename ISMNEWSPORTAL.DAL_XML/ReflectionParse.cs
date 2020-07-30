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
        public List<PropertyValue> GetProperties<T>(T item)
        {
            Type t = typeof(T);
            var properties = t.GetProperties();
            var values = new List<PropertyValue>();
            foreach(PropertyInfo propertyInfo in properties)
            {
                values.Add(new PropertyValue(propertyInfo, item));
            }
            return values;
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
