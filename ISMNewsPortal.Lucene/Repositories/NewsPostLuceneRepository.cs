using ISMNewsPortal.BLL.Models;
using Lucene.Net.Documents;
using System;

namespace ISMNewsPortal.Lucene.Repositories
{
    public class NewsPostLuceneRepository : LuceneRepository<NewsPost>, ILuceneRepository<NewsPost>
    {
        public override NewsPost ConvertTo(Document doc)
        {
            NewsPost newsPost = new NewsPost();
            newsPost.Id = Convert.ToInt32(doc.Get("Id"));
            newsPost.Name = doc.Get("Name");
            newsPost.Description = doc.Get("Description");
            newsPost.ImageId = Convert.ToInt32(doc.Get("ImageId"));
            newsPost.IsVisible = Convert.ToBoolean(doc.Get("IsVisible"));
            newsPost.PublicationDate = DateTools.StringToDate(doc.Get("PublicationDate"));
            return newsPost;
        }

        protected override string[] GetFields()
        {
            return new string[] { "Name", "Description" };
        }

        protected override void PassToIndex(NewsPost item, Document doc)
        {
            doc.Add(new Field("Id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", item.Name, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", item.Description, Field.Store.YES, Field.Index.ANALYZED));
            var dateValue = DateTools.DateToString(item.PublicationDate, DateTools.Resolution.MILLISECOND);
            doc.Add(new Field("PublicationDate", dateValue, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ImageId", item.ImageId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("IsVisible", item.IsVisible.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        }
    }
}
