using RealtorFirm.BLL.Exceptions;
using RealtorFirm.BLL.Interfaces;
using RealtorFirm.BLL.Models;
using RealtorFirm.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorFirm.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepo;

        public ClientService(IRepository<Client> clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public void CreateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.FirstName) || string.IsNullOrWhiteSpace(client.LastName))
            {
                throw new ValidationException("Ім'я та прізвище клієнта є обов'язковими.");
            }
            if (string.IsNullOrWhiteSpace(client.BankAccountNumber))
            {
                throw new ValidationException("Номер банківського рахунку є обов'язковим.");
            }

            _clientRepo.Create(client);
            _clientRepo.SaveChanges();
        }

        public void DeleteClient(int clientId)
        {
            var client = _clientRepo.Get(clientId);
            if (client == null)
            {
                throw new ValidationException("Клієнта не знайдено.");
            }
            _clientRepo.Delete(clientId);
            _clientRepo.SaveChanges();
        }

        public Client GetClient(int clientId)
        {
            var client = _clientRepo.Get(clientId);
            if (client == null)
            {
                throw new ValidationException("Клієнта не знайдено.");
            }
            return client;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _clientRepo.GetAll();
        }

        public IEnumerable<Client> GetSortedClients(string sortBy)
        {
            var list = _clientRepo.GetAll();

            switch (sortBy.ToLower())
            {
                case "firstname": 
                    return list.OrderBy(c => c.FirstName);
                case "lastname": 
                    return list.OrderBy(c => c.LastName);
                case "bankaccount": 
                    return list.OrderBy(c => c.BankAccountNumber);
                default:
                    return list;
            }
        }

        public IEnumerable<Client> SearchClients(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAllClients();
            }

            string lowerKeyword = keyword.ToLower();

            return _clientRepo.GetAll()
                .Where(c => c.FirstName.ToLower().Contains(lowerKeyword) ||
                             c.LastName.ToLower().Contains(lowerKeyword));
        }

        public IEnumerable<Client> AdvancedSearchClients(string lastName, string bankAccount)
        {
            return _clientRepo.GetAll()
                .Where(c => c.LastName == lastName &&
                             c.BankAccountNumber == bankAccount);
        }

        public void UpdateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.FirstName) || string.IsNullOrWhiteSpace(client.LastName))
            {
                throw new ValidationException("Ім'я та прізвище клієнта є обов'язковими.");
            }

            var existing = _clientRepo.Get(client.Id);
            if (existing == null)
            {
                throw new ValidationException("Клієнта для оновлення не знайдено.");
            }

            _clientRepo.Update(client);
            _clientRepo.SaveChanges();
        }
    }
}
