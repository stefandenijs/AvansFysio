using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace AvansFysio.Models.Comments
{
    public class CommentsViewModel
    {
        public string CommentText { get; set; }
        public bool Visible { get; set; }
        public string PlacedBy { get; set; }
        public DateTime Date { get; set; }
    }
}
