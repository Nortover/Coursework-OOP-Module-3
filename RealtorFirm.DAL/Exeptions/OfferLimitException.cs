using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorFirm.BLL.Exceptions
{
    public class OfferLimitException : Exception
    {
        public OfferLimitException(string message) : base(message)
        {
        }
    }
}
