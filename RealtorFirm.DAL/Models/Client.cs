using RealtorFirm.DAL.Entities;
using RealtorFirm.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RealtorFirm.BLL.Models
{
    public class Client : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountNumber { get; set; }

        public List<RealEstate> Offers { get; set; } = new List<RealEstate>();
    }
}