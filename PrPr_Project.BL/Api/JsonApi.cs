using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PrPr_Project.BL.ApiModels;
using PrPr_Project.BL.Interfaces;
using PrPr_Project.DAL.Repositories;
using Group = PrPr_Project.BL.ApiModels.Group;

namespace PrPr_Project.BL.Api
{
    public class JsonApi : IApi
    {
        private const string Uri = "http://localhost:51763/";

        private const string Path = "http://cist.nure.ua/ias/app/tt/";

        private const string Tail = "&ADateStart=18.02.2019&ADateEnd=30.06.2019&AMultiWorkSheet=0";

        public string GetAllGroups()
        {
            const string url = Path + "P_API_GROUP_JSON";
            var result = HttpRequest(url);
            var groups = Parse<Group>(result);
            var list = new List<Group>();
            groups.ForEach(f => list.Add(new Group() {Id = f.id, Name = f.name}));
            return JsonConvert.SerializeObject(list, Formatting.None);
        }

        public string GetAllTeachers()
        {
            const string url = Path + "P_API_PODR_JSON";
            var result = HttpRequest(url);
            var teachers = Parse<Teacher>(result);
            var list = new List<Teacher>();
            teachers.ForEach(f => list.Add(new Teacher()
                {Id = f.id, ShortName = f.short_name, FullName = f.full_name}));
            return JsonConvert.SerializeObject(list, Formatting.None);
        }

        public string GroupSchedule(int id)
        {
            var url = Path + $"WEB_IAS_TT_GNR_RASP.GEN_GROUP_POTOK_RASP?ATypeDoc=3&Aid_group={id}&Aid_potok=0" + Tail;
            var result = HttpRequestCsv(url);
            var list = new List<Lesson>();

            //get subject and teacher list
            var subjects = GetHtmlGroup(id);

            //convert string to lesson representation
            for (var i = 1; i < result.Length - 1; i++)
            {
                var array = result[i].Split(new[] {" , "}, StringSplitOptions.None);
                var info = array[0].Trim().Split(' ');
                var subject = subjects.Find(s => s.ShortSubject.Equals(info[0])) ?? new Subject();

                var teacher = new Teacher();
                foreach (var element in subject.Types)
                {
                    if (element.Key.Equals((info[1])))
                    {
                        teacher = element.Value;
                        break;
                    }
                }

                //create new lesson
                var lesson = new Lesson()
                {
                    GroupId = id,
                    ShortSubject = subject.ShortSubject,
                    LongSubject = subject.LongSubject,
                    Type = info[1],
                    Room = info[2],
                    Group = info[3].Trim(new[] {';', ' ', ','}),
                    Date = array[1].Trim(),
                    Start = array[2].Trim(),
                    End = array[4].Trim(),
                    TeacherId = teacher.Id,
                    ShortTeacher = teacher.ShortName,
                    LongTeacher = teacher.FullName
                };

                list.Add(lesson);
            }

            var lessons = list.OrderBy(e => DateTime.Parse(e.Date)).ThenBy(e => DateTime.Parse(e.Start)).ToList();
            return JsonConvert.SerializeObject(lessons, Formatting.None);
        }

        public string TeacherSchedule(int id)
        {
            var url = Path + $"WEB_IAS_TT_GNR_RASP.GEN_TEACHER_KAF_RASP?ATypeDoc=3&Aid_sotr={id}&Aid_kaf=0" + Tail;
            var result = HttpRequestCsv(url);
            var subjectList = new List<Lesson>();
            var teacherList = (List<Teacher>) JsonConvert.DeserializeObject(GetAllTeachers(), typeof(List<Teacher>));
            var teacher = teacherList.Find(t => t.Id.Equals(id.ToString())) ?? new Teacher();

            //get subject and teacher list
            var subjects = GetHtmlTeacher(id);

            //convert string to lesson representation
            for (var i = 1; i < result.Length - 1; i++)
            {
                var array = result[i].Split(new[] {" , "}, StringSplitOptions.None);
                var info = array[0].Trim().Split(' ');
                var subject = subjects.Find(s => s.ShortSubject.Equals(info[0])) ?? new Subject();

                //create new lesson
                Lesson lesson = null;
                try
                {
                    lesson = new Lesson()
                    {
                        GroupId = id,
                        ShortSubject = subject.ShortSubject,
                        LongSubject = subject.LongSubject,
                        Type = info[1],
                        Room = info[2],
                        Group = info[3].Trim(new[] {';', ' ', ','}),
                        Date = array[1].Trim(),
                        Start = array[2].Trim(),
                        End = array[4].Trim(),
                        TeacherId = teacher.Id,
                        ShortTeacher = teacher.ShortName,
                        LongTeacher = teacher.FullName
                    };
                }
                catch (Exception)
                {
                    // ignored
                }

                if (lesson != null)
                {
                    subjectList.Add(lesson);
                }
            }

            var lessons = subjectList.OrderBy(e => DateTime.Parse(e.Date)).ThenBy(e => DateTime.Parse(e.Start))
                .ToList();
            return JsonConvert.SerializeObject(lessons, Formatting.None);
        }

        public string GetNews()
        {
            var news = new EFUnitOfWork().NewsItems.GetAll().ToList();
            news.ForEach(n => n.Img = Uri + "Api/Image?name=" + n.Img);
            return JsonConvert.SerializeObject(news, Formatting.None);
        }

        public string GetAlternatives(string name = null)
        {
            var alternatives = new EFUnitOfWork().Alternatives.GetAll();
            return JsonConvert.SerializeObject(
                name == null
                    ? alternatives.Take(5).ToList()
                    : alternatives.Where(alternative => alternative.Name.Contains(name)).ToList(), Formatting.None);
        }

        public string GetSeveralSchedules(Dictionary<string, List<string>> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var element in dictionary)
            {
                if (element.Key.Equals("Teacher"))
                {
                    foreach (var value in element.Value)
                    {
                        sb.Append(TeacherSchedule(int.Parse(value)).Trim(new char[] {'[', ']'})).Append(',');
                    }
                }
                else if (element.Key.Equals("Group"))
                {
                    foreach (var value in element.Value)
                    {
                        sb.Append(GroupSchedule(int.Parse(value)).Trim(new char[] {'[', ']'})).Append(',');
                    }
                }
            }

            sb.Replace(',', ']', sb.Length - 1, 1);
            sb.Insert(0, '[');

            var lessons = JsonConvert.DeserializeObject<List<Lesson>>(sb.ToString());
            lessons = lessons.OrderBy(e => DateTime.Parse(e.Date)).ThenBy(e => DateTime.Parse(e.Start))
                .GroupBy(e => new {e.Date, e.Start, e.Group})
                .Select(e => e.First()).ToList();
            return JsonConvert.SerializeObject(lessons, Formatting.None);
        }

        /// <summary>
        /// Http request by specific url
        /// </summary>
        private static string HttpRequest(string url)
        {
            var result = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url);
                response.Wait();
                if (!response.Result.IsSuccessStatusCode) return result;
                var read = response.Result.Content.ReadAsStringAsync();
                read.Wait();
                result = read.Result;
            }

            return result;
        }

        /// <summary>
        /// Parsing string to collection of entity objects
        /// </summary>
        private static List<Entity> Parse<T>(string json)
        {
            var index = 0;
            var entities = new List<Entity>();
            var type = typeof(T).Name.ToLower() + 's';
            while (index < json.Length)
            {
                var firstIndex = json.IndexOf(type, index, StringComparison.Ordinal);
                if (firstIndex == -1)
                {
                    return entities;
                }

                var lastIndex = json.IndexOf("]", firstIndex, StringComparison.Ordinal);
                var start = firstIndex + type.Length + 2;
                var end = lastIndex;
                var length = end - start + 1;
                var result = json.Substring(start, length);
                entities.AddRange(JsonConvert.DeserializeObject<Entity[]>(result));
                index = end + 1;
            }

            return entities;
        }

        /// <summary>
        /// Http request by specific url to get csv-file
        /// </summary>
        private static string[] HttpRequestCsv(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            var response = (HttpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();
            var r = new StreamReader(stream ?? throw new ArgumentNullException(), Encoding.Default);
            var temp = r.ReadToEnd();
            temp = temp.Replace('\"', ' ');
            return temp.Split('\r');
        }

        /// <summary>
        /// Get html representation of schedule for group
        /// </summary>
        private List<Subject> GetHtmlGroup(int groupId)
        {
            //getting .csv-file with schedule using get request to cist.nure.ua
            var result =
                HttpRequest(
                    $"{Path}f?p=778:201:1519161731469535:::201:P201_FIRST_DATE,P201_LAST_DATE,P201_GROUP,P201_POTOK:18.02.2019,30.06.2019,{groupId},0:");
            var rows = GetRowsOfSubjectTable(result);
            var list = new List<Subject>();

            //getting list of teachers
            var buffer = (List<Teacher>) JsonConvert.DeserializeObject(GetAllTeachers(), typeof(List<Teacher>));

            //convert data to subject representation
            for (var i = 1; i < rows.Length; i++)
            {
                var subjectClass = new Subject {Types = new Dictionary<string, Teacher>()};
                var subject = GetNameOfSubject(rows[i]);
                var array = subject.Array;
                subjectClass.ShortSubject = subject.ShortName;
                subjectClass.LongSubject = subject.LongName;

                for (var j = 1; j < array.Length - 1; j++)
                {
                    array[j] = array[j].Trim();

                    const string patternSubject =
                        @"^([А-Яа-яІє]+)(\s.+?\-\s)(.+?)(\,\s+)([А-Яа-яІє]+?\s[А-Яа-яІє]\.\s[А-Яа-яІє]\.)$";
                    var regex = new Regex(patternSubject, RegexOptions.IgnoreCase);
                    var match = regex.Match(array[j]);
                    var groups = match.Groups;
                    var type = groups[1].Value;
                    var teacherName = groups[5].Value;
                    var teacher = buffer.Find(t => t.ShortName.Equals(teacherName)) ?? new Teacher();

                    if (!subjectClass.Types.ContainsKey(type))
                    {
                        subjectClass.Types.Add(type, teacher);
                    }
                }

                list.Add(subjectClass);
            }

            return list;
        }

        /// <summary>
        /// Get rows for subject from html representation of schedule
        /// </summary>
        private static string[] GetRowsOfSubjectTable(string result)
        {
            var startIndex = result.IndexOf("<table class=\"footer\">", StringComparison.Ordinal);
            var endIndex = result.IndexOf("</table>", startIndex, StringComparison.Ordinal);
            var stringTable = result.Substring(startIndex, endIndex - startIndex);
            return stringTable.Split(new[] {"<tr>"}, StringSplitOptions.None);
        }

        /// <summary>
        /// divide html string with subject name on parts
        /// </summary>
        private static (string ShortName, string LongName, string[] Array) GetNameOfSubject(string row)
        {
            const string pattern = "<.+?>";
            row = Regex.Replace(row, pattern, " ", RegexOptions.IgnoreCase).Trim();

            row = row.Replace('\n', ' ');
            var array = row.Split(new[] {" :"}, StringSplitOptions.None);

            var subject = array[0];
            var index = subject.Trim().IndexOf(" ", StringComparison.Ordinal);

            var shortName = subject.Substring(0, index + 1).Trim();
            var longName = subject.Substring(index + 1).Trim();

            return (ShortName: shortName, LongName: longName, Array: array);
        }

        /// <summary>
        /// Get html representation of schedule for teacher
        /// </summary>
        private static List<Subject> GetHtmlTeacher(int teacherId)
        {
            //getting .csv-file with schedule using get request to cist.nure.ua
            var result =
                HttpRequest(
                    Path +
                    $"f?p=778:202:1870379644452667:::202:P202_FIRST_DATE,P202_LAST_DATE,P202_SOTR,P202_KAF:18.02.2019,30.06.2019,{teacherId},0:");
            var rows = GetRowsOfSubjectTable(result);
            var list = new List<Subject>();

            //convert data to subject representation
            for (var i = 1; i < rows.Length; i++)
            {
                var subjectClass = new Subject();
                var (shortName, longName, _) = GetNameOfSubject(rows[i]);
                subjectClass.ShortSubject = shortName;
                subjectClass.LongSubject = longName;
                list.Add(subjectClass);
            }

            return list;
        }
    }
}