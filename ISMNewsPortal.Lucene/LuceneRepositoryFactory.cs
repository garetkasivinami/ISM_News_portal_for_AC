using ISMNewsPortal.BLL.Lucene;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Lucene.Repositories;
using System;
using System.Collections.Generic;

namespace ISMNewsPortal.Lucene
{
    public class LuceneRepositoryFactory : ILuceneRepositoryFactory
    {
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        private string lucenePath;

        public LuceneRepositoryFactory(string lucenePath)
        {
            this.lucenePath = lucenePath;
        }

        public ILuceneRepository<T> GetRepository<T>() where T : Model
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

        private ILuceneRepository<T> GetRepositoryByType<T>() where T : Model
        {
            Type type = typeof(T);
            if (type == typeof(NewsPost))
            {
                return new NewsPostLuceneRepository(lucenePath) as ILuceneRepository<T>;
            }
            throw new Exception("Unknown repository type!");
        }
    }
}
