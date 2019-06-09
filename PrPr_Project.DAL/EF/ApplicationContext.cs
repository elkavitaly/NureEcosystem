using System.Data.Entity;
using PrPr_Project.DAL.Entities;

namespace PrPr_Project.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Alternative> Alternatives { get; set; }

//        public ApplicationContext():base("ProjectDb")
//        {
//            
//        }
    }
}