using System.Web.Mvc;
using PrPr_Project.BL.Api;
using PrPr_Project.BL.Interfaces;

namespace PrPr_Project.WEB.Controllers
{
    public class ScheduleController : Controller
    {
        private IApi Api { get; set; }

        public ScheduleController(JsonApi api) => Api = api;

        public JsonResult GetNews() => Json(Api.GetNews(), JsonRequestBehavior.AllowGet);

        public JsonResult GetAllGroups() => Json(Api.GetAllGroups(), JsonRequestBehavior.AllowGet);

        public JsonResult GetAllTeachers() => Json(Api.GetAllTeachers(), JsonRequestBehavior.AllowGet);

        public JsonResult GroupSchedule(int id) => Json(Api.GroupSchedule(id), JsonRequestBehavior.AllowGet);

        public JsonResult TeacherSchedule(int id) => Json(Api.TeacherSchedule(id), JsonRequestBehavior.AllowGet);

        public JsonResult GetAlternatives(string name) => Json(Api.GetAlternatives(name), JsonRequestBehavior.AllowGet);
    }
}