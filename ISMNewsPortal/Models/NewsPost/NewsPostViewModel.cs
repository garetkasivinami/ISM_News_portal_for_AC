using Antlr.Runtime.Tree;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace ISMNewsPortal.Models
{
    public class NewsPostViewModel
    {
        public NewsPostViewModel(NewsPost newsPost, ICollection<CommentViewModel> comments, int page, int pages, bool actionLinks)
        {
            NewsPost = newsPost;
            Comments = comments;
            Page = page;
            Pages = pages;
            ImagePath = FileModelActions.GetNameByIdFormated(newsPost.ImageId);
            ActionLinks = actionLinks;
        }
        public NewsPost NewsPost { get; set; }
        public string ImagePath { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
        public bool ActionLinks { get; set; }
    }
}