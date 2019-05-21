using PrPr_Project.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrPr_Project.BL.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<AlternativeDTO> GetAll();
        AlternativeDTO GetAlternative(int? id);
        void CreateAlternative(AlternativeDTO item);
        void UpdateAlternative(AlternativeDTO item);
        void DeleteAlternative(int? id);
        void Dispose();
    }
}
