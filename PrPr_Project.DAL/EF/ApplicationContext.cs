using PrPr_Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Alternative> Alternatives { get; set; }
    }
}
