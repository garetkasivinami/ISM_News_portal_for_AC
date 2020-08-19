using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System.Web;

namespace ISMNewsPortal
{
    public class NHibernateSession
    {
        private static ISessionFactory sessionFactory;
        private static object _obj = new object();
        
        public static ISessionFactory SessionFactory
        {
            get
            {
                lock (_obj)
                {
                    if (sessionFactory == null)
                    {
                        var configuration = new Configuration();

                        sessionFactory = CreateSessionFactory(configuration);
                        new SchemaUpdate(configuration).Execute(true, true);
                    }
                }
                return sessionFactory;
            }
        }

        public static ISession Session
        {
            get
            {
                if (!CurrentSessionContext.HasBind(sessionFactory))
                    CurrentSessionContext.Bind(sessionFactory.OpenSession());

                return SessionFactory.GetCurrentSession();
            }
        }

        private static ISessionFactory CreateSessionFactory(Configuration configuration)
        {
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\NHibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);

            var userConfigurationFile = HttpContext.Current.Server.MapPath(@"~\NHibernate\NewsPost.hbm.xml");
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