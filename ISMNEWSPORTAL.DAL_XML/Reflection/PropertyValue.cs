using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Reflection
{
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
