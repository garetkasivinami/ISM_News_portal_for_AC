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
        private static ISessionFactory sessionFactory;
        public static ISession OpenSession()
        {
            if (sessionFactory == null)
            {
                var configuration = new Configuration().SetProperty(NHibernate.Cfg.Environment.UseProxyValidator, bool.FalseString);
                sessionFactory = CreateSessionFactory(configuration);
                new SchemaUpdate(configuration).Execute(true, true);
                return sessionFactory.OpenSession();
            }
            return sessionFactory.OpenSession();
        }

        private static ISessionFactory CreateSessionFactory(Configuration configuration)
        {
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\NHibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);

            string userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\NHibernate\NewsPost.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\NHibernate\Comment.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\NHibernate\Admin.hbm.xml");
            configuration.AddFile(userConfigurationFile);

            userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\NHibernate\FileModel.hbm.xml");
            configuration.AddFile(userConfigurationFile);
            return configuration.BuildSessionFactory();
        }
    }
}