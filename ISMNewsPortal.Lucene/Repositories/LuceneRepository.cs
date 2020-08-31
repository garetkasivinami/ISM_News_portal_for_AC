using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Lucene;
using ISMNewsPortal.BLL.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Version = Lucene.Net.Util.Version;

namespace ISMNewsPortal.Lucene.Repositories
{
    public abstract class LuceneRepository<T> : ILuceneRepository<T> where T : Model
    {
        private static FSDirectory fsdDirectory;

        private const Version LuceneVersion = Version.LUCENE_30;
        protected const Field.Index ANALYZED = Field.Index.ANALYZED;
        protected const Field.Index NOT_ANALYZED = Field.Index.NOT_ANALYZED;

        private readonly IndexWriter.MaxFieldLength DefaultFieldLength;
        public string Directory { get; protected set; }

        private FSDirectory FSDDirectory
        {
            get
            {
                if (fsdDirectory == null)
                    fsdDirectory = FSDirectory.Open(new DirectoryInfo(Directory));

                if (IndexWriter.IsLocked(fsdDirectory)) 
                    IndexWriter.Unlock(fsdDirectory);

                var lockFilePath = Path.Combine(Directory, "write.lock");

                if (File.Exists(lockFilePath)) 
                    File.Delete(lockFilePath);

                return fsdDirectory;
            }
        }

        public LuceneRepository(string path)
        {
            Type type = typeof(T);
            Directory = Path.Combine(HttpRuntime.AppDomainAppPath, path, type.Name);
            DefaultFieldLength = IndexWriter.MaxFieldLength.UNLIMITED;
        }

        public void Delete(IEnumerable<T> items)
        {
            var ids = items.Select(u => u.Id);
            Delete(ids);
        }

        public void Delete(IEnumerable<int> ids)
        {

            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(FSDDirectory, analyzer, DefaultFieldLength))
                {
                    foreach (int id in ids)
                    {
                        var searchQuery = new TermQuery(new Term("Id", id.ToString()));
                        writer.DeleteDocuments(searchQuery);
                    }
                }
            }
        }

        public void Delete(T item)
        {
            Delete(item.Id);
        }

        public void Delete(int id)
        {
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(FSDDirectory, analyzer, DefaultFieldLength))
                {
                    var searchQuery = new TermQuery(new Term("Id", id.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        public void SaveOrUpdate(IEnumerable<T> items)
        {
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(FSDDirectory, analyzer, DefaultFieldLength))
                {
                    foreach (T item in items)
                    {
                        var searchQuery = new TermQuery(new Term("Id", item.Id.ToString()));
                        writer.DeleteDocuments(searchQuery);

                        var doc = new Document();
                        PassToIndex(item, doc);
                        writer.AddDocument(doc);
                    }
                }
            }
        }

        public void SaveOrUpdate(T item)
        {
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(FSDDirectory, analyzer, DefaultFieldLength))
                {
                    var searchQuery = new TermQuery(new Term("Id", item.Id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    var doc = new Document();
                    PassToIndex(item, doc);
                    writer.AddDocument(doc);
                }
            }
        }

        public void Optimize()
        {
            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var writer = new IndexWriter(FSDDirectory, analyzer, DefaultFieldLength))
                {
                    writer.Optimize();
                }
            }
        }

        public bool DeleteAll()
        {
            try
            {
                using (var analyzer = new StandardAnalyzer(LuceneVersion))
                {
                    using (var writer = new IndexWriter(FSDDirectory, analyzer, true, DefaultFieldLength))
                    {
                        writer.DeleteAll();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private List<T> search(string input, Options options)
        {
            if (string.IsNullOrEmpty(input.Replace("*", "").Replace("?", "")))
                return new List<T>();

            using (var analyzer = new StandardAnalyzer(LuceneVersion))
            {
                using (var searcher = new IndexSearcher(FSDDirectory, false))
                {
                    var hits_limit = 1000;
                    QueryParser parser = new MultiFieldQueryParser(LuceneVersion, GetFields(), analyzer);
                    var query = parseQuery(input, parser);

                    Filter filter = GetFilter(options);
                    Sort sort = GetSort(options.SortType, options.Reversed ?? true);
                    var hits = searcher.Search(query, filter, hits_limit, sort).ScoreDocs;

                    return mapLuceneToDataList(hits, searcher);
                }
            }
        }

        private List<T> mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => mapLuceneDocument(searcher.Doc(hit.Doc))).ToList();
        }

        private T mapLuceneDocument(Document doc)
        {
            return ConvertTo(doc);
        }

        public IEnumerable<T> Search(Options options)
        {
            var input = options.Search;
            if (string.IsNullOrEmpty(input))
                return new List<T>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return search(input, options);
        }

        protected abstract T ConvertTo(Document doc);
        protected abstract string[] GetFields();
        protected abstract void PassToIndex(T item, Document doc);
        protected abstract Sort GetSort(string sortType, bool reversed);
        protected abstract Filter GetFilter(Options options);
    }
}
