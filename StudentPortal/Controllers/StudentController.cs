using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Core;
using StudentPortal.Core.Model;
using StudentPortal.Core.Model.Model1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace StudentPortal.Controllers
{
    public class StudentController : Controller
    {
        #region Config
        readonly IStudentPortalServices _studentPortalServices;
        public StudentController(IStudentPortalServices studentPortalServices)
        {
            _studentPortalServices = studentPortalServices;

        }
        #endregion

        #region MainLoginView
        public IActionResult DashBoardLogin()
        {
            return View();
        }
        #endregion

        #region starting poin
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExcelDashBoard()
        {
            return View();
        }

        public IActionResult StudentView()
        {
            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        #endregion

        #region StaffLogin
        public IActionResult StaffLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult StaffLogin(StaffDetails loginDetails)

        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/StaffLogin");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<StaffDetails>(client.BaseAddress, loginDetails);
                postTask.Wait();
                var result = postTask.Result;


                //if (result.IsSuccessStatusCode)
                //{
                //    return RedirectToAction("ViewStudentDetails");
                //}
                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                //return View("StaffLogin");
                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserName", loginDetails.UserName);
                    return RedirectToAction("ViewStudentDetails");
                }
               
                    return RedirectToAction("StaffLogin");
               
            }

        }

        #endregion

        #region StudentLogin
    
        public IActionResult studentLoginView(StudentMarkDetails student)
         {
            return View(student);
         }
        public IActionResult StudentLoginpage(StudentLoginDetails loginDetails)
        {
            return View();
        }
        #endregion

        #region StudentLoge
        public IActionResult StudentLoge(StudentRegistarionDetails loginDetails)
        {
           
               
           
                ExcelMarkDetails markdata = new ExcelMarkDetails();
                if (loginDetails != null)
                {
                    using (var client = new HttpClient())
                    {
                        string name = loginDetails.StudentFirstName;
                        string password = loginDetails.StudentPassword;

                        client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/StudentLogin");
                        var getindividual = client.GetAsync("?name=" + name + "&password=" + password);
                        getindividual.Wait();
                        var result = getindividual.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var individualdata = result.Content.ReadAsAsync<ExcelMarkDetails>();
                            individualdata.Wait();
                        HttpContext.Session.SetString("UserName", loginDetails.StudentFirstName);
                        markdata = individualdata.Result;
                            return RedirectToAction("studentmark", markdata);
                        }
                        else
                        {
                            ViewBag.me = "Wrong credentials";
                            return RedirectToAction("StudentLoginpage");
                        }
                    }

                }
                else
                {
                    return View();
                }
           



        }
      

        public IActionResult StudentLogin(StudentLoginDetails loginDetails)
        {

            //if (ModelState.IsValid)
            //{
            //    ExcelMarkDetails markdata = new ExcelMarkDetails();
            //    using (var client = new HttpClient())
            //    {
            //        string name = loginDetails.studentFisrtName;
            //        string password = loginDetails.studentpassword;

            //        client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/StudentLogin");
            //        var getindividual = client.GetAsync("?name=" + name + "&password=" + password);
            //        getindividual.Wait();
            //        var result = getindividual.Result;
            //        if (result.IsSuccessStatusCode)
            //        {
            //            var individualdata = result.Content.ReadAsAsync<ExcelMarkDetails>();
            //            individualdata.Wait();

            //            markdata = individualdata.Result;
            //            return RedirectToAction("studentLoginView", markdata);
            //        }
            //        else
            //        {
            //            TempData["Wrongcredential"] = "Wrong credentials";
            //            return RedirectToAction("Studentlogin");
            //        }
            //    }
            //}
            //else
            //{
            //    TempData["Wrongcredential"] = "Enter valid credentials";
            //    return RedirectToAction("Studentlogin");
            //}
            return RedirectToAction("StaffLogin");

        }

        #endregion

        #region SaveStudentDetails

        [HttpPost]
        public IActionResult SaveStudentDetails(StudentRegistarionDetails studentRegistarionDetails)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/SaveStudentDetails");
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<StudentRegistarionDetails>(client.BaseAddress, studentRegistarionDetails);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ViewStudentDetails");
                    }
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    return View("SaveStudentDetails");
                }
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }
           
            

         
            //_studentPortalServices.SaveStudentDetails(studentRegistarionDetails);
            //return RedirectToAction("ViewStudentDetails");
        }
        public IActionResult SaveStudentDetails()
        {

            return View();
        }

        #endregion

        #region ViewStudentDetails
        public IActionResult ViewStudentDetails()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {


                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/ViewStudentDetails");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tmp = client.GetAsync(client.BaseAddress).Result;
                    var result = tmp.Content.ReadAsAsync<List<StudentRegistarionDetails>>().Result;

                    return View(result);
                }
                //var list = _studentPortalServices.ViewStudentDetails(studentRegistarionDetails);
                //return View(list);
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }

            
        }
        #endregion

        #region EditStudentDetails
        public IActionResult EditStudentDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                StudentRegistarionDetails studentRegistarionDetails = new StudentRegistarionDetails();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/");
                    //HTTP GET
                    var responseTask = client.GetAsync("EditStudentDetails?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<StudentRegistarionDetails>();
                        readTask.Wait();

                        studentRegistarionDetails = readTask.Result;
                    }
                }

                return View("SaveStudentDetails", studentRegistarionDetails);
                //var editDetails = _studentPortalServices.EditStudentDetails(id);

                //return RedirectToAction("SaveStudentDetails", editDetails);
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }

            

        }
        #endregion

        #region DeleteStudentDetails
        public IActionResult DeleteStudentDetails(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/");
                    //HTTP DELETE
                    var deleteTask = client.DeleteAsync("DeleteStudentDetails?id=" + id);
                    deleteTask.Wait();
                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ViewStudentDetails");
                    }
                }

                return RedirectToAction("ViewStudentDetails");

                //_studentPortalServices.DeleteStudentDetails(id);
                //return RedirectToAction("ViewStudentDetails");
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }
          
        }
        #endregion

        #region FileUplode

        public IActionResult FileUplode(ExcelMarkDetails excelMarkDetails)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/FileUplode");
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ExcelMarkDetails>(client.BaseAddress, excelMarkDetails);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ViewFile");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View("FileUplode");
            }
            else {
                return RedirectToAction("StaffLogin");
            }
           
        }
        #endregion

        #region ViewFile
        public IActionResult ViewFile()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/ViewFile");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tmp = client.GetAsync(client.BaseAddress).Result;
                    var result = tmp.Content.ReadAsAsync<List<ExcelMarkDetails>>().Result;

                    return View(result);
                }
            }

            return RedirectToAction("StaffLogin");
        }
        #endregion

        #region Excel upload post

        [HttpPost]
        public ActionResult Excelupload()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                IFormFile docs = Request.Form.Files["UploadedFile"];

                if (docs != null)
                {
                    Fileupload fileupload = new Fileupload();

                    string filename = Path.GetFileNameWithoutExtension(docs.FileName);
                    string extension = Path.GetExtension(docs.FileName);
                    fileupload.Filename = filename + extension;
                    using (var stream = new MemoryStream())
                    {
                        docs.CopyToAsync(stream);
                        fileupload.filebyte = stream.ToArray();
                    }
                    fileupload.contenttype = docs.ContentType;


                    if (fileupload.Filename.EndsWith(".xls") || fileupload.Filename.EndsWith(".xlsx"))
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/UploadExclel");
                            var Posttask = client.PostAsJsonAsync(client.BaseAddress, fileupload).Result;
                           
                            var checkresult = Posttask.Content.ReadAsAsync<int>().Result;
                            //if (checkresult.IsSuccessStatusCode)
                            //{
                            //    return RedirectToAction("ViewFile");
                            //}
                            if (checkresult == 1)
                            {
                                TempData["ExcelNotify"] = "Enter Correct Student Name";
                                return RedirectToAction("SaveStudentDetails");
                            }
                            if (checkresult == 2)
                            {
                                TempData["ExcelNotify"] = "Enter Correct Roll Number";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 3)
                            {
                                TempData["ExcelNotify"] = "Enter Correct English";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 4)
                            {
                                TempData["ExcelNotify"] = "Enter Correct Maths";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 5)
                            {
                                TempData["ExcelNotify"] = "Enter Correct science";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 6)
                            {
                                TempData["ExcelNotify"] = "Enter Correct Total";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 7)
                            {
                                TempData["ExcelNotify"] = "Enter Correct Avrage";
                                return RedirectToAction("Studentdashboard");
                            }

                            if (checkresult == 8)
                            {
                                TempData["ExcelNotify"] = "select Excel File only";
                                return RedirectToAction("SaveStudentDetails");
                            }

                            if (checkresult == 200)
                            {

                                return RedirectToAction("ViewFile");
                            }



                        }

                    }
                   
                    
                }
                TempData["ExcelNotify"] = "Please select file to upload";
                return RedirectToAction("Studentdashboard");
            }
            else {
                return RedirectToAction("StaffLogin");
            }

           
        }
        
        #endregion

        #region studentmark
        public ActionResult studentmark(StudentMarkDetails student)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return View(student);
            }
            else
            {
                return RedirectToAction("StudentLogin");
            }
        }
        #endregion

        #region Logout
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DashBoardLogin");
        }
        #endregion

        #region Schedule Test
        public IActionResult ScheduleTest()
        {
            return View();
        }

        public IActionResult TestSchedule(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ExcelDetails excelDetails = new ExcelDetails();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/");
                    //HTTP GET
                    var responseTask = client.GetAsync("TestSchedule?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ExcelDetails>();
                        readTask.Wait();

                        excelDetails = readTask.Result;
                    }
                }

                return View("ScheduleTest", excelDetails);
                //var editDetails = _studentPortalServices.EditStudentDetails(id);

                //return RedirectToAction("SaveStudentDetails", editDetails);
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }
        }
        public IActionResult Schedule(ExcelDetails excelsudl)
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44334/StudentPortalApi/Schedule");
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ExcelDetails>(client.BaseAddress, excelsudl);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["MailNotify"] = "Sucessfully sented Test Schedule";
                       
                        return RedirectToAction("ViewStudentDetails");
                    }
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    return View("ScheduleTest");
                }
            }
            else
            {
                return RedirectToAction("StaffLogin");
            }
        }
        #endregion

    }
}
