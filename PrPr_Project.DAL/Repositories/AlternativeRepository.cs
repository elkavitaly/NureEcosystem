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
    public class AlternativeRepository : IRepository<Alternative>
    {
        ApplicationContext db;

        public AlternativeRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public void Create(Alternative item)
        {
            this.db.Alternatives.Add(item);
        }

        public void Delete(int id)
        {
            Alternative book = this.db.Alternatives.Find(id);
            if (book != null)
            {
                this.db.Alternatives.Remove(book);
            }
        }

        public Alternative Get(int id)
        {
            return this.db.Alternatives.Find(id);
        }

        public IEnumerable<Alternative> GetAll()
        {
            return this.db.Alternatives;
        }

        public void Update(Alternative item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}
