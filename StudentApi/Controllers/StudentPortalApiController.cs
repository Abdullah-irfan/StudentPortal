using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Core;
using StudentPortal.Core.Model;
using StudentPortal.Core.Model.Model1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentPortalApiController : ControllerBase
    {
        #region Config
        readonly IStudentPortalServices _studentPortalServices;
        public StudentPortalApiController(IStudentPortalServices studentPortalServices)
        {
            _studentPortalServices = studentPortalServices;
        }
        #endregion


        #region studentlogin
        [HttpGet]
        public IActionResult  StudentLogin(string name, string password)
        {
            var check = _studentPortalServices.StudentLogin(name, password);
            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return NotFound("Not Found");
                //return StatusCode(403); 
            }

        }
        #endregion


        [HttpPost]
        public IActionResult StaffLogin(StaffDetails loginDetails)
        {
            var login = _studentPortalServices.login(loginDetails);
            if (login == true)
            {
                return Ok("sucess");
            }
            return NotFound("Not Found");
        }

        

        [HttpPost]
        public IActionResult SaveStudentDetails(StudentRegistarionDetails studentRegistarionDetails)
        {
            _studentPortalServices.SaveStudentDetails(studentRegistarionDetails);
            return Ok("ViewStudentDetails");
        }
        [HttpPost]
        public IActionResult Schedule(StudentRegistarionDetails excelsudl)
        {
            _studentPortalServices.Schedule(excelsudl);
            return Ok("ViewFile");
        }
            [HttpGet]
        public IActionResult ViewStudentDetails()
        {
            var list = _studentPortalServices.ViewStudentDetails();
            return Ok(list);
        }

        [HttpGet]
        public IActionResult EditStudentDetails(int id)
        {
            var editDetails = _studentPortalServices.EditStudentDetails(id);

            return Ok(editDetails);
        }
        [HttpGet]
        public IActionResult TestSchedule(int id)
        {
            var editDetails = _studentPortalServices.TestSchedule(id);

            return Ok(editDetails);
        }
            [HttpDelete]
        public IActionResult DeleteStudentDetails(int id)
        {
            _studentPortalServices.DeleteStudentDetails(id);
            return Ok("ViewStudentDetails");
        }

        [HttpPost]
        public IActionResult FileUplode(ExcelMarkDetails excelMarkDetails)
        {
            _studentPortalServices.FileUplode(excelMarkDetails);
            return Ok("ViewFile");
        }
        [HttpGet]
        public IActionResult ViewFile()
        {
            var list = _studentPortalServices.ViewFile();
            return Ok(list);
        }

        #region Excel upload file
        [HttpPost]
        public ActionResult UploadExclel(Fileupload fileupload)
        {
            if (fileupload.filebyte != null)
            {
                Fileupload getexcel = new();

                //transform fileupload data into getexcel object
                getexcel.Filename = fileupload.Filename;
                getexcel.contenttype = fileupload.contenttype;

                //as byte[] data not transfering to service and repository flow
                //we convert byte[] to IForm file here...
                byte[] bytefile = fileupload.filebyte;
                var streama = new MemoryStream(bytefile);
                IFormFile file = new FormFile(streama, 0, bytefile.Length, "name", "fileName");

                //here we send IFormFile and Fileupload instanse as arguments...
              var value=  _studentPortalServices.UploadExclel(file, getexcel);
                return Ok(value);
            }
            return Ok();
           

        }

        #endregion
    }
}