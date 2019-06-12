using AutoMapper;
using PrPr_Project.BL.DTO;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.DAL.Entities;
using PrPr_Project.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.BL.Services
{
    public class AdminService : IAdminService
    {
        IUnitOfWork Database { get; set; }

        public AdminService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void CreateAlternative(AlternativeDTO item)
        {
            Alternative alt = new Alternative { Id = item.Id.Value, Name = item.Name, Description = item.Description, Faculty = item.Faculty, Speciality = item.Speciality };
            Database.Alternatives.Create(alt);
            Database.Save();
        }

        public void DeleteAlternative(int? id)
        {
            if (id == null)
                throw new Exception("Не установлено id альтернативы");
            Database.Alternatives.Delete(id.Value);
            Database.Save();
        }

        public IEnumerable<AlternativeDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Alternative, AlternativeDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Alternative>, List<AlternativeDTO>>(Database.Alternatives.GetAll());
        }

        public AlternativeDTO GetAlternative(int? id)
        {
            if (id == null)
                throw new Exception("Не установлено id альтернативы");
            var alt = Database.Alternatives.Get(id.Value);
            if (alt == null)
                throw new Exception("Телефон не найден");

            return new AlternativeDTO { Name = alt.Name, Id = alt.Id, Description = alt.Description, Faculty = alt.Faculty, Speciality = alt.Speciality };
        }

        public void UpdateAlternative(AlternativeDTO item)
        {
            Alternative alt = new Alternative { Id = item.Id.Value, Name = item.Name, Description = item.Description, Faculty = item.Faculty, Speciality = item.Speciality  };
            Database.Alternatives.Update(alt);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
 
        public IEnumerable<NewsItemDTO> GetAllNews()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsItem, NewsItemDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<NewsItem>, List<NewsItemDTO>>(Database.NewsItems.GetAll());
        }

        public NewsItemDTO GetNewsItem(int? id)
        {
            if (id == null)
                throw new Exception("Не установлено id альтернативы");
            var n = Database.NewsItems.Get(id.Value);
            if (n == null)
                throw new Exception("Телефон не найден");

            return new NewsItemDTO { Name = n.Name, Id = n.Id, Author = n.Author, Content = n.Content, Date = n.Date, Img = n.Img };
        }

        public void CreateNewsItem(NewsItemDTO newsItem)
        {
            var n = new NewsItem { Id = newsItem.Id, Name = newsItem.Name, Content = newsItem.Content, Author = newsItem.Author, Date = newsItem.Date, Img = newsItem.Img };
            Database.NewsItems.Create(n);
            Database.Save();
        }
    }
}
