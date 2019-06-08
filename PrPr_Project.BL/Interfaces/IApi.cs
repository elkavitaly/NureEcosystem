using System.Web.Mvc;

namespace PrPr_Project.BL.Interfaces
{
    public interface IApi
    {
        string GetAllGroups();
        string GetAllTeachers();
        string GroupSchedule(int id);
        string TeacherSchedule(int id);
        string GetNews();
        string GetAlternatives(string name = null);
    }
}