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
    public class RealEstateService : IRealEstateService
    {
        private readonly IRepository<RealEstate> _realEstateRepo;
        public RealEstateService(IRepository<RealEstate> realEstateRepo)
        {
            _realEstateRepo = realEstateRepo;
        }

        public void CreateRealEstate(RealEstate realEstate)
        {
            if (realEstate.Cost <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Вартість нерухомості повинна бути > 0.");
            }
            if (string.IsNullOrWhiteSpace(realEstate.Address))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Адреса є обов'язковою.");
            }

            _realEstateRepo.Create(realEstate);
            _realEstateRepo.SaveChanges();
        }

        public void DeleteRealEstate(int realEstateId)
        {
            var realEstate = _realEstateRepo.Get(realEstateId);
            if (realEstate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Об'єкт нерухомості не знайдено.");
            }
            _realEstateRepo.Delete(realEstateId);
            _realEstateRepo.SaveChanges();
        }

        public RealEstate GetRealEstate(int realEstateId)
        {
            var realEstate = _realEstateRepo.Get(realEstateId);
            if (realEstate == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Об'єкт нерухомості не знайдено.");
            }
            return realEstate;
        }

        public IEnumerable<RealEstate> GetAllRealEstate()
        {
            return _realEstateRepo.GetAll();
        }

        public IEnumerable<RealEstate> SearchRealEstate(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAllRealEstate();
            }

            string lowerKeyword = keyword.ToLower();

            return _realEstateRepo.GetAll()
                .Where(re => re.Address.ToLower().Contains(lowerKeyword) ||
                             re.Type.ToString().ToLower().Contains(lowerKeyword));
        }

        public IEnumerable<RealEstate> GetSortedRealEstate(string sortBy)
        {
            var list = _realEstateRepo.GetAll();

            switch (sortBy.ToLower())
            {
                case "type":
                    return list.OrderBy(re => re.Type);
                case "cost":
                    return list.OrderBy(re => re.Cost);
                default:
                    return list;
            }
        }

        public void UpdateRealEstate(RealEstate realEstate)
        {
            if (realEstate.Cost <= 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Вартість нерухомості повинна бути > 0.");
            }

            var existing = _realEstateRepo.Get(realEstate.Id);
            if (existing == null)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Об'єкт нерухомості для оновлення не знайдено.");
            }

            _realEstateRepo.Update(realEstate);
            _realEstateRepo.SaveChanges();
        }
    }
}
