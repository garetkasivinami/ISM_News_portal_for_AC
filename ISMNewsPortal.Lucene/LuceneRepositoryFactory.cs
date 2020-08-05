using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Lucene.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.Lucene
{
    public static class LuceneRepositoryFactory
    {
        private static Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public static ILuceneRepository<T> GetRepository<T>() where T : Model
        {
            Type type = typeof(T);
            if (repositories.ContainsKey(type))
            {
                return repositories[type] as ILuceneRepository<T>;
            }
            ILuceneRepository<T> repository = GetRepositoryByType<T>();
            repositories.Add(type, repository);
            return repository;
        }

        private static ILuceneRepository<T> GetRepositoryByType<T>() where T : Model
        {
            Type type = typeof(T);
            if (type == typeof(NewsPost))
            {
                return new NewsPostLuceneRepository() as ILuceneRepository<T>;
            } else if (type == typeof(Comment))
            {
                return new CommentLuceneRepository() as ILuceneRepository<T>;
            }
            throw new Exception();
        }
    }
}
