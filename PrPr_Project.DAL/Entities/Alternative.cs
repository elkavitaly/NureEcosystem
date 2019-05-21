using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.DAL.Entities
{
    public class Alternative
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
    }
}
