using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.Treatment;
using Core.DomainServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class CommentService : ICommentService
    {
        private readonly PhysioDbContext _context;

        public CommentService(PhysioDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetComments(int patientRecordId)
        {
            var comments = _context.Comments.Where(c => c.PatientRecordId == patientRecordId);
            return comments.ToList();
        }
    }
}
