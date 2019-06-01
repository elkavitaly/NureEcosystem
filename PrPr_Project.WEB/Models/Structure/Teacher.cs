namespace PrPr_Project.WEB.Models.Structure
{
    public class Teacher
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }

        public Teacher()
        {
            Id = 0;
            ShortName = string.Empty;
            FullName = string.Empty;
        }
    }
}