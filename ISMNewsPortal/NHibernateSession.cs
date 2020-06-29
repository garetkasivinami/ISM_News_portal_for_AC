using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);

            var userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\User.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\NewsPost.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Comment.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Admin.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\UserLike.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            new SchemaUpdate(configuration).Execute(true, true);
            return sessionFactory.OpenSession();
        }
    }
}