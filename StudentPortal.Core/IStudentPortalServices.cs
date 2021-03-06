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
    public interface IStudentPortalServices
    {
        public void Schedule(StudentRegistarionDetails excelsudl);
        public StudentRegistarionDetails TestSchedule(int id);
        public int UploadExclel(IFormFile docs, Fileupload getexcel);
    
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
