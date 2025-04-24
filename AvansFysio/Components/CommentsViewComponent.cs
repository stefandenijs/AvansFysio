using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Comments;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class CommentsViewComponent : ViewComponent
    {
        private readonly ICommentService _commentService;

        public CommentsViewComponent(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IViewComponentResult Invoke(int patientRecordId)
        {
            var comments = GetComments(patientRecordId);
            return View(comments);
        }

        private List<CommentsViewModel> GetComments(int patientRecordId)
        {
            var comments= _commentService.GetComments(patientRecordId).ToViewModel();
            comments.Sort((x,y) => DateTime.Compare(x.Date, y.Date));
            return comments;
        }
    }
}
