using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealtorFirm.BLL.Models;

namespace RealtorFirm.BLL.Interfaces
{
    public interface IOfferService
    {
        void AddOfferToClient(int clientId, int realEstateId);
        void RemoveOfferFromClient(int clientId, int realEstateId);
        bool CheckAvailability(RealEstateType type, double cost);
    }
}
