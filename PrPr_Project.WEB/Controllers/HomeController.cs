using System.Collections.Generic;
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
            return View();
        }

        public ActionResult IndexJson()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            IEnumerable<AlternativeDTO> phoneDtos = adminService.GetAll();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AlternativeDTO, AlternativeViewModel>())
                .CreateMapper();
            var alternatives = mapper.Map<IEnumerable<AlternativeDTO>, List<AlternativeViewModel>>(phoneDtos);
            return View(alternatives);
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}