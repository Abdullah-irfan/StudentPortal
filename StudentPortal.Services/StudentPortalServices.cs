using LinqToExcel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentPortal.Services
{
    public class StudentPortalServices : IStudentPortalServices
    {
        #region config
        readonly IStudentPortalRepository _studentPortalRepository;
        public StudentPortalServices(IStudentPortalRepository studentPortalRepository)
        {
            _studentPortalRepository = studentPortalRepository;
        }

        #endregion

        #region Studentslogin
        public ExcelMarkDetails StudentLogin(string name, string password)
        {
             return _studentPortalRepository.StudentLogin(name, password);
        }


        public bool login(StaffDetails loginDetails)
        {
            return _studentPortalRepository.login(loginDetails);
        }

       

        #endregion

        #region SaveStudentDetails
        public void SaveStudentDetails(StudentRegistarionDetails studentRegistarionDetails)
        {
            _studentPortalRepository.SaveStudentDetails(studentRegistarionDetails);
        }
        #endregion

        #region ViewStudentDetails
        public List<StudentRegistarionDetails> ViewStudentDetails()
        {
            return _studentPortalRepository.ViewStudentDetails();
        }
        #endregion

        #region EditStudentDetails
        public StudentRegistarionDetails EditStudentDetails(int id)
        {
            return _studentPortalRepository.EditStudentDetails(id);
        }
        #endregion

        #region DeleteStudentDetails
        public void DeleteStudentDetails(int id)
        {
            _studentPortalRepository.DeleteStudentDetails(id);
        }
        #endregion

        #region ViewFile
        public List<ExcelMarkDetails> ViewFile()
        {
            return _studentPortalRepository.ViewFile();
        }
        #endregion

        #region FileUplode
        public void FileUplode(ExcelMarkDetails excelMarkDetails)
        {
            _studentPortalRepository.FileUplode(excelMarkDetails);
        }
        #endregion

        #region Validate
        public int Validate(List<ExcelDetails> list)
        {
            for (int i = 0; i <= list.Count; i++)
            {
                foreach (var mark in list)
                {
                    if (mark.Student_First_Name != null && mark.Student_Roll_No != null && mark.English != null && mark.Maths != null && mark.Sciences != null && mark.Total != null && mark.Average != null)
                    {

                        var regex = new Regex(@"[^a-zA-Z0-9\s]");
                        var regexs = new Regex(@"[^a-zA-Z0-9\.\s]");
                        var Mark = new Regex(@"[^0-9\.\s]");
                        if (regex.IsMatch(mark.Student_First_Name) || (mark.Student_First_Name.Length < 3 || mark.Student_First_Name.Length > 30))
                        {
                            return 1;
                        }
                        if (regex.IsMatch(mark.Student_Roll_No) || (mark.Student_Roll_No.Length != 4))
                        {
                            return 2;
                        }
                        if (Mark.IsMatch(mark.English) || (Convert.ToInt32(mark.English) > 100) || (Convert.ToInt32(mark.English) < 0))
                        {
                            return 3;
                        }
                        if (Mark.IsMatch(mark.Maths) || (Convert.ToInt32(mark.Maths) > 100) || (Convert.ToInt32(mark.Maths) < 0))
                        {
                            return 4;
                        }
                        if (Mark.IsMatch(mark.Sciences) || (Convert.ToInt32(mark.Sciences) > 100) || (Convert.ToInt32(mark.Sciences) < 0))
                        {
                            return 5;
                        }
                        if (Mark.IsMatch(mark.Total) || (Convert.ToInt32(mark.Total) > 500) || (Convert.ToInt32(mark.Total) < 0))
                        {
                            return 6;
                        }
                        if (Mark.IsMatch(mark.Average) || (Convert.ToDouble(mark.Average) > 150) || (Convert.ToDouble(mark.Average) < 0))
                        {
                            return 7;
                        }

                    }
                    else
                    {
                        return 10;
                    }
                }
            }
            return 200;
        }
        #endregion

        #region UploadExclel
        public int UploadExclel(IFormFile docs, Fileupload getexcel)
        {
            var _context = new StudentportalmanagementEntity();

            string filename = getexcel.Filename;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Root", "Excel", filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                docs.CopyToAsync(fileStream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(fileStream);
                var list = new List<ExcelDetails>();
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                var rowcount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowcount; row++)
                {
                    list.Add(new ExcelDetails
                    {
                        Student_First_Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                        Student_Roll_No = worksheet.Cells[row, 2].Value.ToString().Trim(),
                        English = worksheet.Cells[row, 3].Value.ToString().Trim(),

                        Maths = worksheet.Cells[row, 4].Value.ToString().Trim(),
                        Sciences = worksheet.Cells[row, 5].Value.ToString().Trim(),

                        Total = worksheet.Cells[row, 6].Value.ToString().Trim(),
                        Average = worksheet.Cells[row, 7].Value.ToString().Trim()
                    });
                }
                var value = Validate(list);
                if (value == 200)
                {

                    _studentPortalRepository.UploadExclel(list);
                    return value;
                }
                else
                {
                    return value;
                }
                return 0;
            }

            //if (getexcel.contenttype == "application/vnd.ms-excel" || getexcel.contenttype == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            //{
            //    var connectionString = "";
            //    if (filename.EndsWith(".xls"))
            //    {
            //        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", path);
            //    }
            //    else if (filename.EndsWith(".xlsx"))
            //    {
            //        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", path);
            //    }

            //    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            //    var ds = new DataSet();

            //    adapter.Fill(ds, "ExcelTable");
            //    DataTable dtable = ds.Tables["ExcelTable"];
            //    string sheetName = "Sheet1";
            //    var excelFile = new ExcelQueryFactory(path);
            //    var artistAlbums = from a in excelFile.Worksheet<StudentMarkDetails>(sheetName) select a;

            //    List<StudentMarkDetails> TU = new List<StudentMarkDetails>();
            //    foreach (var mark in artistAlbums)
            //    {
            //        if(mark.Student_First_Name != null && mark.Student_Roll_No !=0 && mark.English !=0  && mark.Maths !=0 && mark.Sciences !=0 && mark.Total !=0 && mark.Average !=0)
            //        {
            //            if (mark.Student_First_Name.Length <= 50 && mark.Student_Roll_No <= 6 && mark.English > 0 && mark.English > 0 && mark.Maths > 0 && mark.Total > 0 && mark.Average > 0 && mark.Total > 0)
            //            {
            //                StudentMarkDetails markupload = new StudentMarkDetails();
            //                markupload.Student_First_Name = mark.Student_First_Name;
            //                markupload.Student_Roll_No = mark.Student_Roll_No;
            //                markupload.English = mark.English;
            //                markupload.Maths = mark.Maths;
            //                markupload.Sciences = mark.Sciences;
            //                markupload.Total = mark.Total;
            //                markupload.Average = mark.Average;
            //                TU.Add(markupload);
            //            }

            //        }




            //    }
            //    _studentPortalRepository.UploadExclel(TU);

        }




        #endregion

        #region Schedule
        public void Schedule(StudentRegistarionDetails excelsudl)
        {
            _studentPortalRepository.Schedule(excelsudl);
        }
        #endregion

        #region TestSchedule
        public StudentRegistarionDetails TestSchedule(int id)
        {
            return _studentPortalRepository.TestSchedule(id);
        }
        #endregion

    }










}


