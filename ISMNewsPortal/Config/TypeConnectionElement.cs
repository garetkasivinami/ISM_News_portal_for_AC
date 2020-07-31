using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Config
{
    public enum TypeConnection
    {
        Hibernate,
        XML
    }
    public class TypeConnectionElement : ConfigurationSection
    {
        [ConfigurationProperty("type", DefaultValue = "hb", IsKey = true, IsRequired = true)]
        public TypeConnection Type
        {
            get { return GetTypeConnection((string)this["type"]); }
            set { this["type"] = GetTypeConnectionString(value); }
        }
        private TypeConnection GetTypeConnection(string type)
        {
            switch(type) 
            {
                case "xml":
                    return TypeConnection.XML;
                default:
                    return TypeConnection.Hibernate;
            }
        }
        private string GetTypeConnectionString(TypeConnection type)
        {
            switch (type)
            {
                case TypeConnection.XML:
                    return "xml";
                default:
                    return "hb";
            }
        }
    }
}