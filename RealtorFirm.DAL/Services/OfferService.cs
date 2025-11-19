using RealtorFirm.BLL.Exceptions;
using RealtorFirm.BLL.Interfaces;
using RealtorFirm.BLL.Models;
using RealtorFirm.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorFirm.BLL.Services
{
    public class OfferService : IOfferService
    {
        private readonly IRepository<Client> _clientRepo;
        private readonly IRepository<RealEstate> _realEstateRepo;

        public OfferService(IRepository<Client> clientRepo, IRepository<RealEstate> realEstateRepo)
        {
            _clientRepo = clientRepo;
            _realEstateRepo = realEstateRepo;
        }

        public void AddOfferToClient(int clientId, int realEstateId)
        {
            var client = _clientRepo.Get(clientId);
            var realEstate = _realEstateRepo.Get(realEstateId);

            if (client == null || realEstate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Клієнта або об'єкт не знайдено.");
            }

            if (client.Offers.Count >= 5)
            {
                throw new OfferLimitException("Клієнт вже має 5 пропозицій. Ліміт досягнуто.");
            }

            if (!client.Offers.Any(o => o.Id == realEstateId))
            {
                client.Offers.Add(realEstate);
                _clientRepo.Update(client);
                _clientRepo.SaveChanges();
            }
        }

        public bool CheckAvailability(RealEstateType type, double cost)
        {
            return _realEstateRepo.GetAll()
                .Any(re => re.Type == type && re.Cost <= cost);
        }

        public void RemoveOfferFromClient(int clientId, int realEstateId)
        {
            var client = _clientRepo.Get(clientId);
            if (client == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Клієнта не знайдено.");
            }

            var offer = client.Offers.FirstOrDefault(o => o.Id == realEstateId);
            if (offer != null)
            {
                client.Offers.Remove(offer);
                _clientRepo.Update(client);
                _clientRepo.SaveChanges();
            }
        }
    }
}
