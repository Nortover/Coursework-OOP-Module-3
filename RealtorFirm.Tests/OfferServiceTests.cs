using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealtorFirm.BLL.Exceptions;
using RealtorFirm.BLL.Interfaces;
using RealtorFirm.BLL.Models;
using RealtorFirm.BLL.Services;
using RealtorFirm.DAL.Interfaces;
using System.Collections.Generic;

namespace RealtorFirm.Tests
{
    [TestClass]
    public class OfferServiceTests
    {
        private Mock<IRepository<Client>> mockClientRepo;
        private Mock<IRepository<RealEstate>> mockRealEstateRepo;
        private IOfferService offerService;

        [TestInitialize]
        public void Setup()
        {
            mockClientRepo = new Mock<IRepository<Client>>();
            mockRealEstateRepo = new Mock<IRepository<RealEstate>>();

            offerService = new OfferService(mockClientRepo.Object, mockRealEstateRepo.Object);
        }

        [TestMethod]
        public void AddOfferToClient_ClientHas5Offers_ShouldThrowOfferLimitException()
        {
            var client = new Client
            {
                Id = 1,
                Offers = new List<RealEstate>
                {
                    new RealEstate { Id = 10 },
                    new RealEstate { Id = 11 },
                    new RealEstate { Id = 12 },
                    new RealEstate { Id = 13 },
                    new RealEstate { Id = 14 }
                }
            };
            var newRealEstate = new RealEstate { Id = 15 };

            mockClientRepo.Setup(r => r.Get(1)).Returns(client);
            mockRealEstateRepo.Setup(r => r.Get(15)).Returns(newRealEstate);

            Assert.ThrowsException<OfferLimitException>(
                () => offerService.AddOfferToClient(1, 15)
            );

            mockClientRepo.Verify(r => r.SaveChanges(), Times.Never());
        }

        [TestMethod]
        public void AddOfferToClient_ClientHas4Offers_ShouldSucceed()
        {
            var client = new Client
            {
                Id = 1,
                Offers = new List<RealEstate> { new RealEstate { Id = 10 },
                    new RealEstate { Id = 11 },
                    new RealEstate { Id = 12 },
                    new RealEstate { Id = 13 }}
            };
            var newRealEstate = new RealEstate { Id = 15 };

            mockClientRepo.Setup(r => r.Get(1)).Returns(client);
            mockRealEstateRepo.Setup(r => r.Get(15)).Returns(newRealEstate);

            offerService.AddOfferToClient(1, 15);

            Assert.AreEqual(5, client.Offers.Count);
            mockClientRepo.Verify(r => r.Update(client), Times.Once());
            mockClientRepo.Verify(r => r.SaveChanges(), Times.Once());
        }
    }
}