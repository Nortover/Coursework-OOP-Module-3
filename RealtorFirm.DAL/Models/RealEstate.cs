using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RealtorFirm.DAL.Entities;

namespace RealtorFirm.BLL.Models
{
    public class RealEstate : BaseEntity
    {
        public RealEstateType Type { get; set; }
        public double Cost { get; set; }
        public string Address { get; set; }
    }
}