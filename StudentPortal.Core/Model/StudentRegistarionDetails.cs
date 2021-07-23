using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core.Model
{
   public class StudentRegistarionDetails
    {
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public DateTime? ScheduleTest { get; set; }
        public string Student_Roll_No { get; set; }
       
        public string StudentRollNumber { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLasttName { get; set; }
        public string StudentGender { get; set; }
        public DateTime StudentDateOfBrith { get; set; }
        public string StudentFatherFirstName { get; set; }
        public string StudentFatherLastName { get; set; }
        public string StudentMotherFirstName { get; set; }
        public string StudentMotherLastName { get; set; }
        public string StudentEmail { get; set; }
        public long StudentContactNo { get; set; }
        public long StudentFatherContactNo { get; set; }
        public string StudentFatherOccupation { get; set; }
        public string StudentUserName { get; set; }
        public string StudentPassword { get; set; }

    }
    public class studentcheck
    {
        public int StudentRollNo { get; set; }
        public string Password { get; set; }
    }
}
