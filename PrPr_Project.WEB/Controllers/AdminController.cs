using AutoMapper;
using PrPr_Project.BL.DTO;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.BL.Services;
using PrPr_Project.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrPr_Project.WEB.Controllers
{
    public class AdminController : Controller
    {
        AdminService adminService;

        public AdminController(AdminService serv)
        {
            adminService = serv;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Main()
        {
            IEnumerable<AlternativeDTO> phoneDtos = adminService.GetAll();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AlternativeDTO, AlternativeViewModel>()).CreateMapper();
            var alternatives = mapper.Map<IEnumerable<AlternativeDTO>, List<AlternativeViewModel>>(phoneDtos);
            return View(alternatives);
        }

        [HttpPost]
        public ActionResult Main(AlternativeViewModel alternative)
        {
            AlternativeDTO alt = new AlternativeDTO() { Name = alternative.Name, Description = alternative.Description, Faculty = alternative.Faculty,
            Speciality = alternative.Speciality, Id = 0 };
            //return Content($@"{alt.Name}, {alt.Description}, {alt.Faculty}, {alt.Speciality}, {alt.Id}");
            adminService.CreateAlternative(alt);
            IEnumerable<AlternativeDTO> phoneDtos = adminService.GetAll();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AlternativeDTO, AlternativeViewModel>()).CreateMapper();
            var alternatives = mapper.Map<IEnumerable<AlternativeDTO>, List<AlternativeViewModel>>(phoneDtos);
            return View(alternatives);
        }


        protected override void Dispose(bool disposing)
        {
            adminService.Dispose();
            base.Dispose(disposing);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangeAlternative(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AlternativeDTO, AlternativeViewModel>()).CreateMapper();
            AlternativeViewModel alt = mapper.Map<AlternativeDTO, AlternativeViewModel>(adminService.GetAlternative(id));
            if (alt != null)
            {
                return View("ChangeAlternative", (object)alt);
            }
            return HttpNotFound();
            
        }

        [HttpPost]
        public ActionResult ChangeAlternative(AlternativeViewModel alt)
        {
            AlternativeDTO alternative = new AlternativeDTO { Id = alt.Id, Speciality = alt.Speciality, Faculty = alt.Faculty, Description = alt.Description, Name = alt.Name };

            adminService.UpdateAlternative(alternative);
            return RedirectToActionPermanent("Main");
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteAlt(int id)
        {
            adminService.DeleteAlternative(id);
            return RedirectToAction("Main");
        }

    }
}