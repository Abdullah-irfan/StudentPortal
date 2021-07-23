using LinqToExcel;
using Microsoft.AspNetCore.Http;
using StudentPortal.Core;
using StudentPortal.Core.Model;
using StudentPortal.Core.Model.Model1;
using StudentPortal.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Repository
{
    public class StudentPortalRepository : IStudentPortalRepository
    {
        
        #region StudentLogin

        public ExcelMarkDetails StudentLogin(string name, string password)
        {

            ExcelMarkDetails markadd = new ExcelMarkDetails();
            using (var _context = new StudentportalmanagementEntity())
            {
                var studetcheck = _context.StudentDetailsDb.Where(m => m.Student_First_Name == name && m.Student_Password == password).FirstOrDefault();
                var marktest = _context.ExcelMarkDb.Where(x => x.Student_First_Name == name).FirstOrDefault();
                if (studetcheck != null && marktest != null)
                {
                    markadd.Student_Roll_No = marktest.Student_Roll_No;
                    markadd.Student_First_Name = marktest.Student_First_Name;
                    markadd.English = marktest.English;
                    markadd.Maths = marktest.Maths;
                    markadd.Sciences = marktest.Sciences;
                    markadd.Total = marktest.Total;
                    markadd.Average = Math.Round((Double)marktest.Average, 2);
                }
                return markadd;
            }
        }

        #endregion

        #region Stafflogin
        public bool login(StaffDetails loginDetails)
        {
            using (StudentportalmanagementEntity entity = new StudentportalmanagementEntity())
            {
                var logindetails = entity.StaffLoginDetailsDb.Where(x => x.Staff_User_Name == loginDetails.UserName && x.Staff_Password == loginDetails.Password).FirstOrDefault();
                if (logindetails != null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region SaveStudentDetails
        public void SaveStudentDetails(StudentRegistarionDetails studentRegistarionDetails)
        {
            StudentDetailsDb studentDetailsDb = new StudentDetailsDb();
            if (studentRegistarionDetails.StudentId == 0)
            {
                using (var entity = new StudentportalmanagementEntity())
                {
                    studentDetailsDb.Student_Id = studentRegistarionDetails.StudentId;
                    studentDetailsDb.Student_Roll_Number = studentRegistarionDetails.StudentRollNumber;
                    studentDetailsDb.Student_First_Name = studentRegistarionDetails.StudentFirstName;
                    studentDetailsDb.Student_Last_Name = studentRegistarionDetails.StudentLasttName;
                    studentDetailsDb.Student_Gender = studentRegistarionDetails.StudentGender;
                    studentDetailsDb.Student_DateOfBrith = studentRegistarionDetails.StudentDateOfBrith;
                    studentDetailsDb.Student_Father_First_Name = studentRegistarionDetails.StudentFatherFirstName;
                    studentDetailsDb.Student_Father_Last_Name = studentRegistarionDetails.StudentFatherLastName;
                    studentDetailsDb.Student_Mother_Fisrt_Name = studentRegistarionDetails.StudentMotherFirstName;
                    studentDetailsDb.Student_Mother_Last_Name = studentRegistarionDetails.StudentMotherLastName;
                    studentDetailsDb.Student_Email_Id = studentRegistarionDetails.StudentEmail;
                    studentDetailsDb.Student_Contact_No = studentRegistarionDetails.StudentContactNo;
                    studentDetailsDb.Student_Father_Contact_No = studentRegistarionDetails.StudentFatherContactNo;
                    studentDetailsDb.Father_Occupation = studentRegistarionDetails.StudentFatherOccupation;
                    studentDetailsDb.Student_User_Name = studentRegistarionDetails.StudentUserName;
                    studentDetailsDb.Student_Password = studentRegistarionDetails.StudentPassword;
                    studentDetailsDb.Is_Deleted = false;
                    entity.StudentDetailsDb.Add(studentDetailsDb);
                    entity.SaveChanges();

                }

            }
            else
            {
                using (var entity = new StudentportalmanagementEntity())
                {
                    var studentdbData = entity.StudentDetailsDb.Where(x => x.Student_Id == studentRegistarionDetails.StudentId && x.Is_Deleted == false).SingleOrDefault();
                    if (studentdbData != null)
                    {
                        studentdbData.Student_Id = studentRegistarionDetails.StudentId;
                        studentdbData.Student_First_Name = studentRegistarionDetails.StudentFirstName;
                        studentdbData.Student_Last_Name = studentRegistarionDetails.StudentLasttName;
                        studentdbData.Student_Gender = studentRegistarionDetails.StudentGender;
                        studentdbData.Student_DateOfBrith = studentRegistarionDetails.StudentDateOfBrith;
                        studentdbData.Student_Father_First_Name = studentRegistarionDetails.StudentFatherFirstName;
                        studentdbData.Student_Father_Last_Name = studentRegistarionDetails.StudentFatherLastName;
                        studentdbData.Student_Mother_Fisrt_Name = studentRegistarionDetails.StudentMotherFirstName;
                        studentdbData.Student_Mother_Last_Name = studentRegistarionDetails.StudentMotherLastName;
                        studentdbData.Student_Email_Id = studentRegistarionDetails.StudentEmail;
                        studentdbData.Student_Contact_No = studentRegistarionDetails.StudentContactNo;
                        studentdbData.Student_Father_Contact_No = studentRegistarionDetails.StudentFatherContactNo;
                        studentdbData.Father_Occupation = studentRegistarionDetails.StudentFatherOccupation;
                        studentdbData.Student_User_Name = studentRegistarionDetails.StudentUserName;
                        studentdbData.Student_Password = studentRegistarionDetails.StudentPassword;
                        studentdbData.Is_Deleted = false;
                        entity.SaveChanges();
                    }
                }
            }
        }

        #endregion

        #region FileUplode
        public void FileUplode(ExcelMarkDetails excelMarkDetails)
        {
            //get file name
            var filename = ContentDispositionHeaderValue.Parse(excelMarkDetails.ExcelFile.ContentDisposition).FileName.Trim('"');

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }

            //get file path 
            var filePath = Path.Combine(MainPath, excelMarkDetails.ExcelFile.FileName);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                excelMarkDetails.ExcelFile.CopyToAsync(stream);
            }

            //get extension
            string extension = Path.GetExtension(filename);


            string connectionString = string.Empty;

            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filePath);
                    break;
                case ".xlsx": //Excel 07 and above.
                    connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", filePath);
                    break;
            }
            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            var ds = new DataSet();
            adapter.Fill(ds, "ExcelTable");
            DataTable dtable = ds.Tables["ExcelTable"];
            string sheetName = "Sheet1";
            var excelFile = new ExcelQueryFactory(filePath);
            var artistAlbums = from a in excelFile.Worksheet<ExcelMarkDetails>(sheetName) select a;
            using (var entity = new StudentportalmanagementEntity())
                foreach (var a in artistAlbums)
                {

                    ExcelMarkDb TU = new ExcelMarkDb();
                    //TU.Student_Roll_No = a.username;
                    TU.English = a.English;
                    TU.Maths = a.Maths;
                    TU.Sciences = a.Sciences;
                    TU.Total = a.Total;
                    TU.Average = a.Average;
                    entity.ExcelMarkDb.Add(TU);
                    entity.SaveChanges();

                }
        }


        #endregion

        #region ViewStudentDetails
        public List<StudentRegistarionDetails> ViewStudentDetails()
        {
            List<StudentRegistarionDetails> studentRegistarions = new List<StudentRegistarionDetails>();
            using (var entity = new StudentportalmanagementEntity())
            {
                var studentDetails = entity.StudentDetailsDb.Where(x => x.Is_Deleted == false).ToList();
                if (studentDetails != null)
                {
                    foreach (var student in studentDetails)
                    {
                        StudentRegistarionDetails studentRegistarionDetails = new StudentRegistarionDetails();
                        studentRegistarionDetails.StudentId = student.Student_Id;
                        studentRegistarionDetails.StudentRollNumber = student.Student_Roll_Number;
                        studentRegistarionDetails.StudentFirstName = student.Student_First_Name;
                        studentRegistarionDetails.StudentLasttName = student.Student_Last_Name;
                        studentRegistarionDetails.StudentGender = student.Student_Gender;
                        studentRegistarionDetails.StudentDateOfBrith = student.Student_DateOfBrith;
                        studentRegistarionDetails.StudentFatherFirstName = student.Student_Father_First_Name;
                        studentRegistarionDetails.StudentFatherLastName = student.Student_Father_Last_Name;
                        studentRegistarionDetails.StudentMotherFirstName = student.Student_Mother_Fisrt_Name;
                        studentRegistarionDetails.StudentMotherLastName = student.Student_Mother_Last_Name;
                        studentRegistarionDetails.StudentEmail = student.Student_Email_Id;
                        studentRegistarionDetails.StudentContactNo = student.Student_Contact_No;
                        studentRegistarionDetails.StudentFatherContactNo = student.Student_Father_Contact_No;
                        studentRegistarionDetails.StudentFatherOccupation = student.Father_Occupation;
                        studentRegistarionDetails.StudentUserName = student.Student_User_Name;
                        studentRegistarionDetails.StudentPassword = student.Student_Password;

                        studentRegistarions.Add(studentRegistarionDetails);

                    }
                }

            }
            return studentRegistarions;
        }

        #endregion

        #region ViewFile
        public List<ExcelMarkDetails> ViewFile()
        {
            List<ExcelMarkDetails> excels = new List<ExcelMarkDetails>();
            using (var entity = new StudentportalmanagementEntity())
            {
                var ExcelFileDetails = entity.ExcelMarkDb.Where(x => x.Is_Deleted == false).ToList();
                if (ExcelFileDetails != null)
                {
                    foreach (var excelMarkDb in ExcelFileDetails)
                    {
                        ExcelMarkDetails excelMarkDetails = new ExcelMarkDetails();
                        //excelMarkDetails.username = excelMarkDb.User_Name;
                        excelMarkDetails.Student_First_Name = excelMarkDb.Student_First_Name;
                        excelMarkDetails.Student_Roll_No = excelMarkDb.Student_Roll_No;
                        excelMarkDetails.English = excelMarkDb.English;
                        excelMarkDetails.Maths = excelMarkDb.Maths;
                        excelMarkDetails.Sciences = excelMarkDb.Sciences;
                        excelMarkDetails.Total = excelMarkDb.Total;
                        excelMarkDetails.Average =  Math.Round((Double)excelMarkDb.Average, 2);
                        excels.Add(excelMarkDetails);

                    }
                }

            }
            return excels;
        }

        #endregion

        #region EditStudentDetails

        public StudentRegistarionDetails EditStudentDetails(int id)
        {
            StudentRegistarionDetails studentDetails = new StudentRegistarionDetails();
            using (var entity = new StudentportalmanagementEntity())
            {
                var studentDb = entity.StudentDetailsDb.Where(x => x.Student_Id == id && x.Is_Deleted == false).FirstOrDefault();
                if (studentDb != null)
                {
                    studentDetails.StudentId = studentDb.Student_Id;
                    studentDetails.StudentRollNumber = studentDb.Student_Roll_Number;
                    studentDetails.StudentFirstName = studentDb.Student_First_Name;
                    studentDetails.StudentLasttName = studentDb.Student_Last_Name;
                    studentDetails.StudentGender = studentDb.Student_Gender;
                    studentDetails.StudentDateOfBrith = studentDb.Student_DateOfBrith;
                    studentDetails.StudentFatherFirstName = studentDb.Student_Father_First_Name;
                    studentDetails.StudentFatherLastName = studentDb.Student_Father_Last_Name;
                    studentDetails.StudentMotherFirstName = studentDb.Student_Mother_Fisrt_Name;
                    studentDetails.StudentMotherLastName = studentDb.Student_Mother_Last_Name;
                    studentDetails.StudentEmail = studentDb.Student_Email_Id;
                    studentDetails.StudentContactNo = studentDb.Student_Contact_No;
                    studentDetails.StudentFatherContactNo = studentDb.Student_Father_Contact_No;
                    studentDetails.StudentFatherOccupation = studentDb.Father_Occupation;
                    studentDetails.StudentUserName = studentDb.Student_User_Name;
                    studentDetails.StudentPassword = studentDb.Student_Password;
                }
                return studentDetails;

            }
        }
        #endregion

        #region DeleteStudentDetails

        public void DeleteStudentDetails(int id)
        {
            StudentRegistarionDetails studentRegistarion = new StudentRegistarionDetails();
            using (var entites = new StudentportalmanagementEntity())
            {
                var studentData = entites.StudentDetailsDb.Where(x => x.Student_Id == id).SingleOrDefault();
                if (studentData != null)
                {
                    studentData.Is_Deleted = true;
                    studentData.Updated_Time_Stamp = DateTime.Now;
                    entites.SaveChanges();
                }
            }
        }

        #endregion

        #region UploadExclel

      
        public void UploadExclel(List<ExcelDetails> excelDetails)
        {
            var _context = new StudentportalmanagementEntity();
            
            foreach (var mark in excelDetails)

                {
             
                bool isExisting = false;
              var  a = Convert.ToInt32(mark.Student_Roll_No);
                var markupload = _context.ExcelMarkDb.Where(x => x.Student_Roll_No == a && x.Is_Deleted == false).SingleOrDefault();
                if(markupload != null)
                {
                    isExisting = true;
                }
                else
                {
                    markupload = new ExcelMarkDb();
                }

                    markupload.Student_First_Name = mark.Student_First_Name;
                    markupload.Student_Roll_No = Convert.ToInt32(mark.Student_Roll_No);
                    markupload.English = Convert.ToInt32(mark.English);
                markupload.Maths = Convert.ToInt32(mark.Maths);
                markupload.Sciences = Convert.ToInt32(mark.Sciences);
                markupload.Total = Convert.ToInt32(mark.Total);
                markupload.Average = Convert.ToDouble(mark.Average);
             

                if (isExisting == false)
                {
                    _context.ExcelMarkDb.Add(markupload);
                }
                   
                    _context.SaveChanges();

                }



            

        }



        #endregion

        #region Schedule

        
        public void Schedule(StudentRegistarionDetails excelsudl)
        {
            StudentDetailsDb excelDb = new StudentDetailsDb();
            if (excelsudl.StudentId == 0)
            {
                using (var entity = new StudentportalmanagementEntity())
                {
                    excelDb.Student_Id = excelsudl.StudentId;
                    excelDb.Subject = excelsudl.Subject;
                    excelDb.ScheduleTest = excelsudl.ScheduleTest;

                    excelDb.Is_Deleted = false;
                    entity.StudentDetailsDb.Add(excelDb);
                    entity.SaveChanges();

                }

            }
            else
            {
                using (var entity = new StudentportalmanagementEntity())
                {
                    var studentdbData = entity.StudentDetailsDb.Where(x => x.Student_Id == excelsudl.StudentId && x.Is_Deleted == false).SingleOrDefault();
                    if (studentdbData != null)
                    {
                        studentdbData.Student_Id = excelsudl.StudentId;
                        studentdbData.Subject = excelsudl.Subject;
                        studentdbData.ScheduleTest = excelsudl.ScheduleTest;
                        studentdbData.Is_Deleted = false;
                        entity.SaveChanges();
                    }
                }
            }
        }

        public StudentRegistarionDetails TestSchedule(int id)
        {
            StudentRegistarionDetails excelDetails = new StudentRegistarionDetails();
            using (var entity = new StudentportalmanagementEntity())
            {
                var excelDb = entity.StudentDetailsDb.Where(x => x.Student_Id == id && x.Is_Deleted == false).FirstOrDefault();
                if (excelDb != null)
                {
                    excelDetails.StudentId = excelDb.Student_Id;
                    excelDetails.Student_Roll_No = excelDb.Student_Roll_Number;
                    excelDetails.Subject = excelDb.Subject;
                    excelDetails.ScheduleTest = excelDb.ScheduleTest;
                }
                return excelDetails;

            }
        }
        #endregion
    }

}