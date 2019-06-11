using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrPr_Project.WEB.Models
{
    public class NewsItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}