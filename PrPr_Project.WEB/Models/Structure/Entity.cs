using System.Collections.Generic;
using System.Data;

namespace PrPr_Project.WEB.Models.Structure
{
    public class Entity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string full_name { get; set; }

        public override string ToString() =>
            $"{{\"Id\": {id}, \"ShortName\":\"{short_name}\", \"FullName\":\"{full_name}\"}}";
    }
}