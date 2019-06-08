namespace PrPr_Project.BL.ApiModels
{
    public class Lesson
    {
        public int GroupId { get; set; }
        public string Group { get; set; }
        public string ShortSubject { get; set; }
        public string LongSubject { get; set; }
        public string Type { get; set; }
        public string Room { get; set; }
        public string Date { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string ShortTeacher { get; set; }
        public string LongTeacher { get; set; }

        public override string ToString()
        {
            return
                $"{GroupId}, {Group}, {ShortSubject}, {Type}, {Room}, {Date}, {Start}, {End}, {ShortTeacher}, {LongTeacher}, {LongSubject}";
        }
    }
}