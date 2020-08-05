using ISMNewsPortal.BLL.Models;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.Lucene.Repositories
{
    class CommentLuceneRepository : LuceneRepository<Comment>, ILuceneRepository<Comment>
    {
        protected override string[] GetFields()
        {
            return new string[] { "UserName", "Text" };
        }

        protected override void PassToIndex(Comment item, Document doc)
        {
            doc.Add(new Field("Id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("UserName", item.UserName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Text", item.Text, Field.Store.YES, Field.Index.ANALYZED));
        }
    }
}
