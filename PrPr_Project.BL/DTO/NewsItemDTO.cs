using System;

namespace PrPr_Project.BL.DTO
{
    public class NewsItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}