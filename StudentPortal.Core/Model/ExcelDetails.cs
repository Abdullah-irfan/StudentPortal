using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core.Model
{
  public  class ExcelDetails
    {

        public int StudentId { get; set; }
        public string Student_First_Name { get; set; }
        public string Student_Roll_No { get; set; }
        public string English { get; set; }
        public string Maths { get; set; }
        public string Sciences { get; set; }

        public string Subject { get; set; }

        public DateTime? ScheduleTest { get; set; }
        public string Total { get; set; }
        public string Average { get; set; }
    }
}
