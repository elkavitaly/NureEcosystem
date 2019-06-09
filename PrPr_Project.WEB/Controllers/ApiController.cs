using System.Web.Mvc;
using PrPr_Project.BL.Api;
using PrPr_Project.BL.Interfaces;

namespace PrPr_Project.WEB.Controllers
{
    public class ApiController : Controller
    {
        private IApi Api { get; set; }

        public ApiController(JsonApi api) => Api = api;

        public JsonResult News() => Json(Api.GetNews(), JsonRequestBehavior.AllowGet);

        public JsonResult AllGroups() => Json(Api.GetAllGroups(), JsonRequestBehavior.AllowGet);

        public JsonResult AllTeachers() => Json(Api.GetAllTeachers(), JsonRequestBehavior.AllowGet);

        public JsonResult GroupSchedule(int id) => Json(Api.GroupSchedule(id), JsonRequestBehavior.AllowGet);

        public JsonResult TeacherSchedule(int id) => Json(Api.TeacherSchedule(id), JsonRequestBehavior.AllowGet);

        public JsonResult Alternatives(string name) => Json(Api.GetAlternatives(name), JsonRequestBehavior.AllowGet);
    }
}