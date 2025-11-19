using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealtorFirm.BLL.Models;

namespace RealtorFirm.BLL.Interfaces
{
    public interface IClientService
    {
        void CreateClient(Client client);
        void DeleteClient(int clientId);
        void UpdateClient(Client client);
        Client GetClient(int clientId);
        IEnumerable<Client> GetAllClients();
        IEnumerable<Client> GetSortedClients(string sortBy);
        IEnumerable<Client> SearchClients(string keyword);
        IEnumerable<Client> AdvancedSearchClients(string lastName, string bankAccount);
    }
}
