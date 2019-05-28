using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PrPr_Project.WEB.Models.Structure;

namespace PrPr_Project.WEB.Controllers
{
    public class ScheduleController : Controller
    {
        private const string Uri = "http://localhost:51763/";

        private const string Path = "http://cist.nure.ua/ias/app/tt/";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetFacultiesJson()
        {
            const string request = Path + "P_API_FACULTIES_JSON";
            var result = "NotFound";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(request);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var read = response.Result.Content.ReadAsStringAsync();
                    read.Wait();
                    result = read.Result;
                }
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);
            var faculties = ConvertJson(json, "faculties");

            return Json(faculties, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirectionsJson(string facultyName)
        {
            var faculties = JsonConvert.DeserializeObject<Entity[]>(GetFacultiesJson().ToString());
            var facultyId = faculties.First(faculty => faculty.short_name.Equals(facultyName)).id;

            var request = Path + "P_API_DIRECTIONS_JSON?p_id_faculty=" + facultyId;
            var result = "NotFound";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(request);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var read = response.Result.Content.ReadAsStringAsync();
                    read.Wait();
                    result = read.Result;
                }
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);
            var directions = ConvertJson(json, "directions");
            return Json(directions, JsonRequestBehavior.AllowGet);
        }

        public Entity[] ConvertJson(JsonResult json, string type)
        {
            var result = json.Data.ToString();
            var start = result.IndexOf(type, StringComparison.Ordinal) + type.Length + 2;
            var end = result.IndexOf("]", start, StringComparison.Ordinal);
            var length = end - start;
            result = result.Substring(start, length + 1);
            return JsonConvert.DeserializeObject<Entity[]>(result);
        }

        public Entity[] ConvertJson(JsonResult json) =>
            JsonConvert.DeserializeObject<Entity[]>(GetFacultiesJson().ToString());

        public JsonResult GetAll(string query)
        {
            var request = Path;
            var type = "";

            if (query == "groups")
            {
                request += "P_API_GROUP_JSON";
                type = "groups";
            }
            else if (query == "teachers")
            {
                request += "P_API_PODR_JSON";
                type = "teachers";
            }

            var result = "NotFound";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(request);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var read = response.Result.Content.ReadAsStringAsync();
                    read.Wait();
                    result = read.Result;
                }
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);
            var faculties = ParseGroups(result, type);
            if (type == "groups")
            {
                var list = new List<Group>();
                faculties.ForEach(f => list.Add(new Group() {name = f.name, id = f.id}));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = new List<Teacher>();
                faculties.ForEach(f => list.Add(new Teacher()
                    {id = f.id, short_name = f.short_name, full_name = f.full_name}));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public List<Entity> ParseGroups(string json, string type)
        {
            var gIndex = 0;
            var groups = new List<Entity>();

            while (gIndex < json.Length)
            {
                var firstIndex = json.IndexOf(type, gIndex, StringComparison.Ordinal);
                if (firstIndex == -1)
                {
                    return groups;
                }

                var lastIndex = json.IndexOf("]", firstIndex, StringComparison.Ordinal);
                var start = firstIndex + type.Length + 2;
                var end = lastIndex;
                var length = end - start + 1;
                var result = json.Substring(start, length);
                groups.AddRange(JsonConvert.DeserializeObject<Entity[]>(result));
                gIndex = end + 1;
            }

            return groups;
        }

        public void Get()
        {
            var result = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("http://cist.nure.ua/ias/app/tt/f?p=778:201:1519161731469535:::201:P201_FIRST_DATE,P201_LAST_DATE,P201_GROUP,P201_POTOK:01.02.2019,30.07.2019,6496576,0:");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var read = response.Result.Content.ReadAsStringAsync();
                    read.Wait();
                    result = read.Result;
                }
            }
        }
    }
}