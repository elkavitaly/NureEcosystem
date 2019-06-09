namespace PrPr_Project.BL.ApiModels
{
    public class Teacher
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }

        public Teacher()
        {
            Id = "0";
            ShortName = string.Empty;
            FullName = string.Empty;
        }
    }
}