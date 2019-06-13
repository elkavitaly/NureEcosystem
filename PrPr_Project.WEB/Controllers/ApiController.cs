using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using PrPr_Project.BL.Api;
using PrPr_Project.DAL.Entities;

namespace PrPr_Project.WEB.Controllers
{
    public class ApiController : Controller
    {
        private JsonApi Api { get; set; }

        public ApiController(JsonApi api) => Api = api;

        public ActionResult Image(string name)
        {
            using (new HttpResponseMessage())
            {
                var directory = Server.MapPath("/Content/Images");
                var path = Path.Combine(directory, name);
                return File(path, "image/jpeg");
            }
        }

        //        public JsonResult News()
        //        {
        //            return Json(Api.GetNews(), JsonRequestBehavior.AllowGet);
        //        }

        public async Task<JsonResult> News()
        {
            var path = Path.Combine(Server.MapPath("../Content/Data"), "News.json");
            using (var stream = new StreamReader(path))
            {
                var json = stream.ReadToEnd();
                return await Task.Run(() => Json(json, JsonRequestBehavior.AllowGet));
            }
        }

        public async Task<JsonResult> AllGroups()
        {
            return await Task.Run(() => Json(Api.GetAllGroups(), JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> AllTeachers()
        {
            return await Task.Run(() => Json(Api.GetAllTeachers(), JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> GroupSchedule()
        {
            return await Task.Run(() =>
                Json(Api.GroupSchedule(Deserialize<int>(Request.InputStream)), JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> TeacherSchedule()
        {
            return await Task.Run(() =>
                Json(Api.TeacherSchedule(Deserialize<int>(Request.InputStream)), JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> SeveralSchedules()
        {
            return await Task.Run(() =>
                Json(Api.GetSeveralSchedules(Deserialize<Dictionary<string, List<string>>>(Request.InputStream)),
                    JsonRequestBehavior.AllowGet));
        }

        //        public JsonResult Alternatives(string name)
        //        {
        //            return Json(Api.GetAlternatives(name), JsonRequestBehavior.AllowGet);
        //        }

        public JsonResult Alternatives(string name)
        {
            var path = Path.Combine(Server.MapPath("../Content/Data"), "Alternatives.json");
            string json;
            using (var stream = new StreamReader(path))
            {
                json = stream.ReadToEnd();
            }

            var list = JsonConvert.DeserializeObject(json, typeof(Alternative));
            //            var list = Deserialize<AlternativeViewModel>(new StreamReader(path).BaseStream);

            //            var serialized = JsonConvert.SerializeObject(name == null
            //                ? list.Take(5).ToList()
            //                : list.Where(alternative => alternative.Name.Contains(name)).ToList(), Formatting.None);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public string Http()
        {
            var dict = new Dictionary<string, List<string>>
            {
                {"Teacher", new List<string>() {"9", "6437405"}},
                {"Group", new List<string>() {"6283375", "6283365"}}
            };
            var data = JsonConvert.SerializeObject("МСП");
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.Default, "application/json");
                var response = client.PostAsync("http://localhost:51763/Api/Alternatives", content);
                var result = response.Result.Content.ReadAsStringAsync();
                return "Your request:\n" + result.Result;
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            string content;
            using (var streamReader = new StreamReader(stream))
            {
                content = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}