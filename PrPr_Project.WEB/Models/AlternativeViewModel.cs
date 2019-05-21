using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrPr_Project.WEB.Models
{
    public class AlternativeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
    }
}