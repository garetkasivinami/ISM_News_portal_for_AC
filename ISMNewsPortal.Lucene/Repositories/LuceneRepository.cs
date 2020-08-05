using ISMNewsPortal.BLL.BusinessModels;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Version = Lucene.Net.Util.Version;

namespace ISMNewsPortal.Lucene.Repositories
{
    public abstract class LuceneRepository<T> : ILuceneRepository<T> where T : Model
    {
        private static FSDirectory _directoryTemp;

        private string directory;
        public string Directory
        {
            get => directory;
        }

        private FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(Directory));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(Directory, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        public LuceneRepository()
        {
            Type type = typeof(T);
            directory = Path.Combine(HttpRuntime.AppDomainAppPath, "lucene_index", type.Name);
        }

        public void Delete(IEnumerable<T> items)
        {
            
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (T item in items)
                    {
                        var searchQuery = new TermQuery(new Term("Id", item.Id.ToString()));
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
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term("Id", id.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        public void SaveOrUpdate(IEnumerable<T> items)
        {
            foreach (T item in items)
                SaveOrUpdate(item);
        }

        protected abstract void PassToIndex(T item, Document doc);

        public void SaveOrUpdate(T item)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
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
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                }
            }
        }

        public bool DeleteAll()
        {
            try
            {
                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {
                    using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
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

        protected abstract string[] GetFields();

        private List<int> search(string input)
        {
            if (string.IsNullOrEmpty(input.Replace("*", "").Replace("?", "")))
                return new List<int>();

            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var hits_limit = 1000;
                    QueryParser parser = new MultiFieldQueryParser(Version.LUCENE_30, GetFields(), analyzer);
                    var query = parseQuery(input, parser);

                    var hits = searcher.Search(query, hits_limit).ScoreDocs;

                    return _mapLuceneToDataList(hits, searcher);
                }
            }
        }

        private static List<int> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToDataId(searcher.Doc(hit.Doc))).ToList();
        }

        private static int _mapLuceneDocumentToDataId(Document doc)
        {
            return Convert.ToInt32(doc.Get("Id"));
        }

        public IEnumerable<int> Search(Options options)
        {
            var input = options.Search;
            if (string.IsNullOrEmpty(input))
                return new List<int>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            List<int> results = search(input);
            return results;
        }
    }
}
