using ISMNewsPortal.BLL.Models;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.Lucene.Repositories
{
    public class NewsPostLuceneRepository : LuceneRepository<NewsPost>, ILuceneRepository<NewsPost>
    {
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
        }
    }
}
