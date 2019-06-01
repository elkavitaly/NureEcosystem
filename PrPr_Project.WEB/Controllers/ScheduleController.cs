using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PrPr_Project.WEB.Models.Structure;
using Group = PrPr_Project.WEB.Models.Structure.Group;

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
                    {Id = f.id, ShortName = f.short_name, FullName = f.full_name}));
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

        public List<Subject> Get(int groupId)
        {
            var result = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response =
                    client.GetAsync(
                        "http://cist.nure.ua/ias/app/tt/f?p=778:201:1519161731469535:::201:P201_FIRST_DATE,P201_LAST_DATE,P201_GROUP,P201_POTOK:01.02.2019,30.07.2019," +
                        groupId + ",0:");
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var read = response.Result.Content.ReadAsStringAsync();
                    read.Wait();
                    result = read.Result;
                }
            }

            var startIndex = result.IndexOf("<table class=\"footer\">", StringComparison.Ordinal);
            var endIndex = result.IndexOf("</table>", startIndex, StringComparison.Ordinal);
            var stringTable = result.Substring(startIndex, endIndex - startIndex);
            var rows = stringTable.Split(new string[] {"<tr>"}, StringSplitOptions.None);


            var list = new List<Subject>();
            const string pattern = "<.+?>";
            var temp = string.Empty;

            var buffer = (List<Teacher>) GetAll("teachers").Data;
//            var teachers = JsonConvert.DeserializeObject<Entity>(buffer.ToString());

            for (var i = 1; i < rows.Length; i++)
            {
                var subjectClass = new Subject();
                subjectClass.Types = new Dictionary<string, Teacher>();
                rows[i] = Regex.Replace(rows[i], pattern, " ", RegexOptions.IgnoreCase).Trim();
                rows[i] = rows[i].Replace('\n', ' ');
                var array = rows[i].Split(new string[] {" :"}, StringSplitOptions.None);
                var subject = array[0];
                var index = subject.Trim().IndexOf(" ", StringComparison.Ordinal);

                var shortName = subject.Substring(0, index + 1).Trim();
                var longName = subject.Substring(index + 1).Trim();

                subjectClass.LongSubject = longName;
                subjectClass.ShortSubject = shortName;

                for (var j = 1; j < array.Length - 1; j++)
                {
                    array[j] = array[j].Trim();
                    var spaceIndex = array[j].IndexOf(" ", StringComparison.Ordinal);
                    var type = array[j].Substring(0, spaceIndex + 1).Trim();

                    var comaIndex = array[j].LastIndexOf(",", StringComparison.Ordinal);
                    var teacherShort = array[j].Substring(comaIndex + 1).Trim();

                    var teacher = Regex.IsMatch(teacherShort, @"^.+?\s.\.\s.\.$")
                        ? buffer.Find(t => t.ShortName.Equals(teacherShort))
                        : new Teacher();

                    if (!subjectClass.Types.ContainsKey(type))
                    {
                        subjectClass.Types.Add(type, teacher);
                    }
                }

                list.Add(subjectClass);
            }

            return list;
        }

        public JsonResult CsvConvert(int groupId)
        {
            // reading .csv file from stream using http request
            var url = "http://cist.nure.ua/ias/app/tt/WEB_IAS_TT_GNR_RASP.GEN_GROUP_POTOK_RASP?ATypeDoc=3&Aid_group=" +
                      groupId + "&Aid_potok=0&ADateStart=18.02.2019&ADateEnd=30.06.2019&AMultiWorkSheet=0";
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            var response = (HttpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();
            var r = new StreamReader(stream, Encoding.Default);
            var temp = r.ReadToEnd();

            temp = temp.Replace('\"', ' ');
            var result = temp.Split('\r');
            var list = new List<Lesson>();

            var subjects = Get(groupId);
            for (var i = 1; i < result.Length - 1; i++)
            {
                var array = result[i].Split(new string[] {" , "}, StringSplitOptions.None);
                var info = array[0].Trim().Split(' ');


                var subject = subjects.Find(s => s.ShortSubject.Equals(info[0]));
                if (subject == null)
                {
                    subject = new Subject();
                }

                Teacher teacher = null;
                foreach (var element in subject.Types)
                {
                    if (element.Key.Equals((info[1])))
                    {
                        teacher = element.Value;
                        break;
                    }
                }

                if (teacher == null)
                {
                    teacher = new Teacher();
                }

                var lesson = new Lesson()
                {
                    GroupId = groupId,
                    ShortSubject = subject.ShortSubject,
                    LongSubject = subject.LongSubject,
                    Type = info[1],
                    Room = info[2],
                    Group = info[3],
                    Date = array[1].Trim(),
                    Start = array[2].Trim(),
                    End = array[4].Trim(),
                    ShortTeacher = teacher.ShortName,
                    LongTeacher = teacher.FullName
                };

                list.Add(lesson);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}