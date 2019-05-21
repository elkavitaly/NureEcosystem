using PrPr_Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<NewsItem> NewsItems { get; }
        IRepository<Alternative> Alternatives { get; }
        void Save();
    }
}
