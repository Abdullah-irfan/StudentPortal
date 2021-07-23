﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentPortal.Entity
{
    public partial class ExcelMarkDb
    {
        [Key]
        public int Student_Id { get; set; }
        [StringLength(30)]
        public string Student_First_Name { get; set; }
        public int Student_Roll_No { get; set; }
        public int English { get; set; }
        public int Maths { get; set; }
        public int Sciences { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }
        public bool Is_Deleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Inserted_Time_Stamp { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Updated_Time_Stamp { get; set; }
    }
}