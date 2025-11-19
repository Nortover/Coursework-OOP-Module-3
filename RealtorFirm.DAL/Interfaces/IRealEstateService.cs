using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealtorFirm.BLL.Models;

namespace RealtorFirm.BLL.Interfaces
{
    public interface IRealEstateService
    {
        void CreateRealEstate(RealEstate realEstate);
        void DeleteRealEstate(int realEstateId);
        void UpdateRealEstate(RealEstate realEstate);
        RealEstate GetRealEstate(int realEstateId);
        IEnumerable<RealEstate> GetAllRealEstate();
        IEnumerable<RealEstate> GetSortedRealEstate(string sortBy);
        IEnumerable<RealEstate> SearchRealEstate(string keyword);
    }
}
