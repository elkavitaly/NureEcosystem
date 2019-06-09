namespace PrPr_Project.BL.ApiModels
{
    public class Entity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string full_name { get; set; }

        public override string ToString() =>
            $"{{\"Id\": {id}, \"ShortName\":\"{short_name}\", \"FullName\":\"{full_name}\"}}";
    }
}