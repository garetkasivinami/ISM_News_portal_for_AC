using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Config
{
    public class CustomSettingsGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("typeConnection", IsRequired = true)]
        public TypeConnectionElement TypeConnectionElement
        {
            get
            {
                return (TypeConnectionElement)base.Sections["typeConnection"];
            }
        }
    }
}