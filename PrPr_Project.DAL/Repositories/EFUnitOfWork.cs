using PrPr_Project.DAL.EF;
using PrPr_Project.DAL.Entities;
using PrPr_Project.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;
        private NewsItemRepository newsItemRepository;
        private AlternativeRepository alternativeRepository;

        public EFUnitOfWork()
        {
            db = new ApplicationContext();
        }

        public IRepository<NewsItem> NewsItems {
            get
            {
                if (newsItemRepository == null)
                    newsItemRepository = new NewsItemRepository(db);
                return newsItemRepository;
            }
        }

        public IRepository<Alternative> Alternatives {
            get
            {
                if (alternativeRepository == null)
                    alternativeRepository = new AlternativeRepository(db);
                return alternativeRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}
