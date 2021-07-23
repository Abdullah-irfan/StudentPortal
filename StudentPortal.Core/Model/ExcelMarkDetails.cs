using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Core.Model
{
   public class ExcelMarkDetails
    {
        public IFormFile ExcelFile { get; set; }
        public int Student_Roll_No { get; set; }

        public string Student_First_Name { get; set; }
        public int English { get; set; }
        public int Maths { get; set; }
        public int Sciences { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }

        

    }

    
}
