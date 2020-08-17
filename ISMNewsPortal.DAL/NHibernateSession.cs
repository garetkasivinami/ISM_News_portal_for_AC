using NHibernate;
using NHibernate.Cfg;
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
                        var configuration = new Configuration()
                            .SetProperty(Environment.UseProxyValidator, bool.FalseString)
                            .SetProperty(Environment.CurrentSessionContextClass, "thread_static");

                        sessionFactory = CreateSessionFactory(configuration);
                        new SchemaUpdate(configuration).Execute(true, true);
                    }
                }
                return sessionFactory;
            }
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