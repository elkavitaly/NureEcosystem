using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using PrPr_Project.BL.Api;

namespace PrPr_Project.WEB.Controllers
{
    public class ApiController : Controller
    {
        private JsonApi Api { get; set; }

        public ApiController(JsonApi api) => Api = api;

        public ActionResult Image(string name)
        {
            using (var response = new HttpResponseMessage())
            {
                var directory = Server.MapPath("/Content/Images");
                var path = Path.Combine(directory, name);
                return File(path, "image/jpeg");
            }
        }

        public JsonResult News() => Json(Api.GetNews(), JsonRequestBehavior.AllowGet);

        public JsonResult AllGroups() => Json(Api.GetAllGroups(), JsonRequestBehavior.AllowGet);

        public JsonResult AllTeachers() => Json(Api.GetAllTeachers(), JsonRequestBehavior.AllowGet);

        public JsonResult GroupSchedule()
        {
            return Json(Api.GroupSchedule(Deserialize<int>(Request.InputStream)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult TeacherSchedule()
        {
            return Json(Api.TeacherSchedule(Deserialize<int>(Request.InputStream)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManyGroups()
        {
            var data = Deserialize<Dictionary<string, List<string>>>(Request.InputStream);
            var sb = new StringBuilder();
            foreach (var element in data)
            {
                if (element.Key.Equals("Teacher"))
                {
                    foreach (var value in element.Value)
                    {
                        sb.Append(Api.TeacherSchedule(int.Parse(value)).Trim(new char[] {'[', ']'}));
                    }
                }
                else if (element.Key.Equals("Group"))
                {
                    foreach (var value in element.Value)
                    {
                        sb.Append(Api.GroupSchedule(int.Parse(value))).Append(',');
                    }
                }
            }

            sb.Insert(0, '[').Insert(sb.Length - 1, ']');

            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Alternatives(string name) => Json(Api.GetAlternatives(name), JsonRequestBehavior.AllowGet);

        public string Http(int id)
        {
//            var dict = new Dictionary<string, List<string>>
//            {
//                {"Teacher", new List<string>() {"1754070", "6437405"}},
//                {"Group", new List<string>() {"6496576", "6496579"}}
//            };
//            var data = JsonConvert.SerializeObject(dict);
            var data = id.ToString();
            using (var client = new HttpClient())
            {
                var content = new StringContent(data, Encoding.Default, "application/json");
                var response = client.PostAsync("http://localhost:51763/Api/GroupSchedule", content);
                var result = response.Result.Content.ReadAsStringAsync();
                return "Your request:\n" + result.Result;
            }
        }

        private static T Deserialize<T>(Stream stream)
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