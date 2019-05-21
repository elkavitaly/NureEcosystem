using PrPr_Project.DAL.EF;
using PrPr_Project.DAL.Entities;
using PrPr_Project.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.DAL.Repositories
{
    public class NewsItemRepository : IRepository<NewsItem>
    {
        ApplicationContext db;

        public NewsItemRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public void Create(NewsItem item)
        {
            this.db.NewsItems.Add(item);
        }

        public void Delete(int id)
        {
            NewsItem book = this.db.NewsItems.Find(id);
            if (book != null)
            {
                this.db.NewsItems.Remove(book);
            }
        }

        public NewsItem Get(int id)
        {
            return this.db.NewsItems.Find(id);
        }

        public IEnumerable<NewsItem> GetAll()
        {
            return this.db.NewsItems;
        }

        public void Update(NewsItem item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}
