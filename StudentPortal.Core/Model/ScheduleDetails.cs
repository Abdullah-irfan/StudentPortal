using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core.Model
{
    public class ScheduleDetails
    {
        public int StudentId { get; set; }
        public int RollNumber { get; set; }
        public string Subject { get; set; }

        public DateTime ScheduleTime { get; set; }
    }
}
