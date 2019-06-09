using System;

namespace PrPr_Project.DAL.Entities
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Img { get; set; }
    }
}