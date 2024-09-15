using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Survey.Contract.Constent.Enums;

namespace Survey.Contract.Model
{
    public class SurveyAssignmentDTO
    {
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public DateTime AssignedDate { get; set; }
        public AssigneStatusType Status { get; set; } = AssigneStatusType.NotStarted;
    }
}
