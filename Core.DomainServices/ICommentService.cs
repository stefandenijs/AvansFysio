using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.Treatment;

namespace Core.DomainServices
{
    public interface ICommentService
    {
        public List<Comment> GetComments(int patientRecordId);
    }
}
