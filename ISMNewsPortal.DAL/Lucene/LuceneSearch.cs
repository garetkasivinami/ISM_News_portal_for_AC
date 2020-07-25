using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Repositories;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace ISMNewsPortal.DAL.Lucene
{
    public static class LuceneSearch
    {
        private static string _luceneDir =
                Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");
        private static FSDirectory _directoryTemp;
        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        private static void _addToLuceneIndex(NewsPost sampleData, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", sampleData.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            var doc = new Document();

            doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", sampleData.Name, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));
            var dateValue = DateTools.DateToString(sampleData.PublicationDate, DateTools.Resolution.MILLISECOND);
            doc.Add(new Field("PublicationDate", dateValue, Field.Store.YES, Field.Index.NOT_ANALYZED));

            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<NewsPost> sampleDatas)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var sampleData in sampleDatas)
                        _addToLuceneIndex(sampleData, writer);
                }
            }
        }

        public static void ClearLuceneIndexRecord(int record_id)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        public static bool ClearLuceneIndex()
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

        public static void Optimize()
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                }
            }
        }

        private static int _mapLuceneDocumentToDataId(Document doc)
        {
            return Convert.ToInt32(doc.Get("Id"));
        }

        private static IEnumerable<int> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToDataId).ToList();
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

        private static List<NewsPost> _search(string searchQuery, DateTime minFilterDate, DateTime maxFilterDate, SortField sortField = null, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<NewsPost>();
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var hits_limit = 1000;
                    IEnumerable<int> results;
                    QueryParser parser;
                    if (!string.IsNullOrEmpty(searchField))
                    {
                        parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    }
                    else
                    {
                        parser = new MultiFieldQueryParser
                            (Version.LUCENE_30, new[] { "Id", "Name", "Description", "PublicationDate" }, analyzer);
                    }
                    var query = parseQuery(searchQuery, parser);
                    var minDate = DateTools.DateToString(minFilterDate, DateTools.Resolution.SECOND);
                    var maxDate = DateTools.DateToString(maxFilterDate, DateTools.Resolution.SECOND);
                    var filter = FieldCacheRangeFilter.NewStringRange("PublicationDate", minDate, maxDate, true, true);

                    Sort sort = Sort.RELEVANCE;
                    if (sortField != null)
                        sort = new Sort(sortField);

                    var hits = searcher.Search(query, filter, hits_limit, sort).ScoreDocs;

                    results = _mapLuceneToDataList(hits, searcher);
                    List<NewsPost> newsPosts = new List<NewsPost>();
                    return newsPosts;
                }
            }
        }

        public static IEnumerable<NewsPost> Search(string input, string fieldName, DateTime? minFilterDate = null, DateTime? maxFilterDate = null, bool reverse = false)
        {
            if (string.IsNullOrEmpty(input)) 
                return new List<NewsPost>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            List<NewsPost> results = _search(input, minFilterDate ?? new DateTime(0), maxFilterDate ?? DateTime.Now, new SortField(fieldName, SortField.STRING, reverse));
            return results;
        }

        public static IEnumerable<NewsPost> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<NewsPost>() : _search(input, new DateTime(0), DateTime.Now);
        }

        private static IEnumerable<int> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToDataId(searcher.Doc(hit.Doc))).ToList();
        }

        public static void AddUpdateLuceneIndex(NewsPost sampleData)
        {
            AddUpdateLuceneIndex(new List<NewsPost> { sampleData });
        }
    }
}
