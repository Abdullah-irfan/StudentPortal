using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core.Model.Model1
{
   public class StudentInformationDetails
    {
        public int StudentId { get; set; }
        public int StudentRollNo { get; set; }
        
        public string StudentFirstName { get; set; }
       
        public string StudentLastName { get; set; }
      
        public string Gender { get; set; }
    
        public DateTime Dob { get; set; }
     
        public string FatherFirstName { get; set; }
       
        public string FatherLastName { get; set; }
    
        public string MotherFirstName { get; set; }
      
        public string MotherLastName { get; set; }
       
        public string Email { get; set; }
  
        public long StudentContactNo { get; set; }
       
        public long FatherSContactNo { get; set; }
  
        public string FatherSOccupation { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
       
        public string Password { get; set; }

        public bool IsMarkadded { get; set; }
        public IFormFile Excel { get; set; }
    }
    public class studentcheck
    {
        public int StudentRollNo { get; set; }
        public string Password { get; set; }
    }

    public class Fileupload
    {
        public IFormFile excelfile { get; set; }
        public string Filename { get; set; }
        public byte[] filebyte { get; set; }
        public string contenttype { get; set; }
    }
}
