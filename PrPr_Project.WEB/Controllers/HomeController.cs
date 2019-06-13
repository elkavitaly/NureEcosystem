using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using PrPr_Project.BL.DTO;
using PrPr_Project.BL.Services;
using PrPr_Project.WEB.Models;

namespace PrPr_Project.WEB.Controllers
{
    public class HomeController : Controller
    {
        AdminService adminService;

        public HomeController(AdminService serv)
        {
            adminService = serv;
        }

        public ActionResult Index()
        {
            var newsDtos = adminService.GetAllNews();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsItemDTO, NewsItemViewModel>())
                .CreateMapper();
            var newsItems = mapper.Map<IEnumerable<NewsItemDTO>, List<NewsItemViewModel>>(newsDtos);
            newsItems = newsItems.OrderByDescending(x => x.Date).ToList();
            return View(newsItems);
        }

        public ActionResult IndexJson()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            var phoneList = adminService.GetAll();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AlternativeDTO, AlternativeViewModel>())
                .CreateMapper();
            var alternatives = mapper.Map<IEnumerable<AlternativeDTO>, List<AlternativeViewModel>>(phoneList);
            return View(alternatives);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(NewsItemViewModel newsItem)
        {
            var n = new NewsItemDTO
            {
                Id = newsItem.Id, Name = newsItem.Name, Content = newsItem.Content, Author = newsItem.Author,
                Date = newsItem.Date, Img = newsItem.Img
            };

            n.Content = string.Join("<br>", newsItem.Content.Split('\n'));

            adminService.CreateNewsItem(n);

            return RedirectToAction("CreateNews");
        }

        public ActionResult Preview(int id)
        {
            var n = adminService.GetNewsItem(id);

            var newsItem = new NewsItemViewModel
            {
                Id = n.Id,
                Author = n.Author,
                Content = n.Content,
                Date = n.Date,
                Name = n.Name,
                Img = n.Img
            };

            ViewBag.Content = new MvcHtmlString(n.Content);

            return View(newsItem);
        }

        [HttpPost]
        public JsonResult Sort()
        {
            var boxes = ApiController.Deserialize<List<string>>(Request.InputStream);
            var list = adminService.Filter(boxes);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<NewsItemDTO, NewsItemViewModel>())
                .CreateMapper();
            var news = mapper.Map<IEnumerable<NewsItemDTO>, List<NewsItemViewModel>>(list);
            return Json(news, JsonRequestBehavior.AllowGet);
        }
    }
}