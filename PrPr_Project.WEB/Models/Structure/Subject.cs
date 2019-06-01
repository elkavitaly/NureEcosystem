using System.Collections.Generic;

namespace PrPr_Project.WEB.Models.Structure
{
    public class Subject
    {
        public string ShortSubject { get; set; }
        public string LongSubject { get; set; }
        public Dictionary<string, Teacher> Types { get; set; }

        public Subject()
        {
            ShortSubject = string.Empty;
            LongSubject = string.Empty;
            Types = new Dictionary<string, Teacher>();
        }
    }
}