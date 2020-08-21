using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Lucene;
using ISMNewsPortal.BLL.Models;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Text.RegularExpressions;

namespace ISMNewsPortal.Lucene.Repositories
{
    public class NewsPostLuceneRepository : LuceneRepository<NewsPost>, ILuceneRepository<NewsPost>
    {
        public NewsPostLuceneRepository(string path) : base(path)
        {

        }

        protected override NewsPost ConvertTo(Document doc)
        {
            NewsPost newsPost = new NewsPost();
            newsPost.Id = Convert.ToInt32(doc.Get("Id"));
            newsPost.Name = doc.Get("Name");
            newsPost.Description = doc.Get("Description");
            newsPost.ImageId = Convert.ToInt32(doc.Get("ImageId"));
            newsPost.AuthorId = Convert.ToInt32(doc.Get("AuthorId"));
            newsPost.IsVisible = Convert.ToBoolean(doc.Get("IsVisible"));
            newsPost.PublicationDate = DateTools.StringToDate(doc.Get("PublicationDate"));
            newsPost.EditDate = DateTools.StringToDate(doc.Get("EditDate"));
            newsPost.CreatedDate = DateTools.StringToDate(doc.Get("CreatedDate"));
            return newsPost;
        }

        protected override Filter GetFilter(Options options)
        {
            string minDate = DateTools.DateToString(options.MinimumDate ?? DateTime.MinValue, DateTools.Resolution.MILLISECOND);
            string maxDate = DateTools.DateToString(options.MaximumDate ?? DateTime.MaxValue, DateTools.Resolution.MILLISECOND);
            return FieldCacheRangeFilter.NewStringRange("PublicationDate",
                minDate, maxDate,
                true, true);
        }

        protected override Sort GetSort(string sortType, bool reversed)
        {
            sortType = sortType?.ToLower();
            SortField sortField;
            switch (sortType)
            {
                case "id":
                    sortField = new SortField("Id", SortField.INT, reversed);
                    break;
                case "name":
                    sortField = new SortField("Name", SortField.STRING, reversed);
                    break;
                case "description":
                    sortField = new SortField("Description", SortField.STRING, reversed);
                    break;
                case "editdate":
                    sortField = new SortField("EditDate", SortField.STRING, reversed);
                    break;
                case "author":
                    sortField = new SortField("AuthorId", SortField.INT, reversed);
                    break;
                case "publicationdate":
                    sortField = new SortField("PublicationDate", SortField.STRING, reversed);
                    break;
                case "visibility":
                    sortField = new SortField("IsVisible", SortField.STRING, reversed);
                    break;
                default:
                    sortField = new SortField("CreatedDate", SortField.STRING, reversed);
                    break;
            }
            return new Sort(sortField);
        }

        protected override string[] GetFields()
        {
            return new string[] {"Id", "Name", "Description" };
        }

        protected override void PassToIndex(NewsPost item, Document doc)
        {
            doc.Add(new Field("Id", item.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("AuthorId", item.AuthorId.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Name", item.Name, Field.Store.YES, Field.Index.ANALYZED));
            string description = Regex.Replace(item.Description, "<.*?>", String.Empty);
            doc.Add(new Field("Description", description, Field.Store.YES, Field.Index.ANALYZED));
            var publicationDate = DateTools.DateToString(item.PublicationDate, DateTools.Resolution.MILLISECOND);
            doc.Add(new Field("PublicationDate", publicationDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
            var editDate = DateTools.DateToString(item.EditDate ?? DateTime.MinValue, DateTools.Resolution.MILLISECOND);
            doc.Add(new Field("EditDate", editDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
            var createdDate = DateTools.DateToString(item.CreatedDate, DateTools.Resolution.MILLISECOND);
            doc.Add(new Field("CreatedDate", createdDate, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ImageId", item.ImageId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("IsVisible", item.IsVisible.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        }
    }
}
