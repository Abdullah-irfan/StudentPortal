using Microsoft.AspNetCore.Http;
using StudentPortal.Core.Model;
using StudentPortal.Core.Model.Model1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core
{
    public interface IStudentPortalRepository
    {
        //public void ScheduleTest(StudentRegistarionDetails studentRegistarionDetails);
        public StudentRegistarionDetails TestSchedule(int id);
        public void Schedule(StudentRegistarionDetails excelsudl);
        public void UploadExclel(List<ExcelDetails> markDetails);
        
        public bool login(StaffDetails loginDetails);

        public ExcelMarkDetails StudentLogin(string name, string password);

        public void DeleteStudentDetails(int id);
        public StudentRegistarionDetails EditStudentDetails(int id);
        public void SaveStudentDetails(StudentRegistarionDetails studentRegistarionDetails);
        public List<StudentRegistarionDetails> ViewStudentDetails();
        public List<ExcelMarkDetails> ViewFile();
        void FileUplode(ExcelMarkDetails excelMarkDetails);


    }
}
