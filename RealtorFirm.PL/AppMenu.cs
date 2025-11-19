using RealtorFirm.BLL.Exceptions;
using RealtorFirm.BLL.Interfaces;
using RealtorFirm.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorFirm.PL
{
    public class AppMenu
    {
        private readonly IClientService _clientService;
        private readonly IRealEstateService _realEstateService;
        private readonly IOfferService _offerService;

        public AppMenu(IClientService clientService, IRealEstateService realEstateService, IOfferService offerService)
        {
            _clientService = clientService;
            _realEstateService = realEstateService;
            _offerService = offerService;
        }

        public void Run()   
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Ріелтерська фірма ---");
                Console.WriteLine("1. Керування клієнтами");
                Console.WriteLine("2. Керування нерухомістю");
                Console.WriteLine("3. Керування пропозиціями");
                Console.WriteLine("0. Вихід");
                Console.Write("> ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ClientMenu();
                        break;
                    case "2":
                        RealEstateMenu();
                        break;
                    case "3":
                        OfferMenu();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Невірна команда.");
                        break;
                }
                if (running)
                {
                    Console.WriteLine("\nНатисніть Enter для продовження...");
                    Console.ReadLine();
                }
            }
        }
        private void ClientMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Клієнти ---");
            Console.WriteLine("1. Додати клієнта");
            Console.WriteLine("2. Показати всіх клієнтів");
            Console.WriteLine("3. Знайти клієнта");
            Console.Write("> ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddClient();
                    break;
                case "2":
                    ShowAllClients();
                    break;
                case "3":
                    SearchClients();
                    break;
            }
        }

        private void AddClient()
        {
            try
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine();
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine();
                Console.Write("Номер рахунку: ");
                string bankAccount = Console.ReadLine();

                var client = new Client
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BankAccountNumber = bankAccount
                };

                _clientService.CreateClient(client);

                Console.WriteLine("Клієнта успішно додано.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Помилка валідації: {ex.Message}");
            }
        }

        private void ShowAllClients()
        {
            var clients = _clientService.GetAllClients();
            Console.WriteLine("--- Список клієнтів ---");
            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id}, {client.FirstName} {client.LastName}, Рахунок: {client.BankAccountNumber}");
                if (client.Offers.Count > 0)
                {
                    Console.WriteLine("  Пропозиції:");
                    foreach (var offer in client.Offers)
                    {
                        Console.WriteLine($"    - {offer.Address} (ID: {offer.Id})");
                    }
                }
            }
        }

        private void SearchClients()
        {
            Console.Write("Введіть ключове слово (ім'я/прізвище): ");
            string keyword = Console.ReadLine();
            var clients = _clientService.SearchClients(keyword);

            Console.WriteLine("--- Результати пошуку ---");
            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id}, {client.FirstName} {client.LastName}");
            }
        }
        private void RealEstateMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Нерухомість ---");
            Console.WriteLine("1. Додати об'єкт");
            Console.WriteLine("2. Показати всі об'єкти");
            Console.Write("> ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddRealEstate();
                    break;
                case "2":
                    ShowAllRealEstate();
                    break;
            }
        }

        private void AddRealEstate()
        {
            try
            {
                Console.WriteLine("Оберіть тип:");
                Console.WriteLine("0 - 1-кімнатна");
                Console.WriteLine("1 - 2-кімнатна");
                Console.WriteLine("2 - 3-кімнатна");
                Console.WriteLine("3 - Приватна ділянка");
                Console.Write("> ");

                if (!Enum.TryParse(Console.ReadLine(), out RealEstateType type) || !Enum.IsDefined(typeof(RealEstateType), type))
                {
                    Console.WriteLine("Невірний тип.");
                    return;
                }

                Console.Write("Адреса: ");
                string address = Console.ReadLine();

                Console.Write("Вартість: ");
                if (!double.TryParse(Console.ReadLine(), out double cost))
                {
                    Console.WriteLine("Невірна вартість.");
                    return;
                }

                var realEstate = new RealEstate
                {
                    Type = type,
                    Address = address,
                    Cost = cost
                };

                _realEstateService.CreateRealEstate(realEstate);
                Console.WriteLine("Об'єкт нерухомості успішно додано.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Помилка валідації: {ex.Message}");
            }
        }

        private void ShowAllRealEstate()
        {
            var items = _realEstateService.GetAllRealEstate();
            Console.WriteLine("--- Список нерухомості ---");
            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Id}, {item.Type}, {item.Address}, Ціна: {item.Cost:C}");
            }
        }

        private void OfferMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Пропозиції ---");
            Console.WriteLine("1. Додати пропозицію клієнту");
            Console.WriteLine("2. Видалити пропозицію у клієнта");
            Console.WriteLine("3. Перевірити наявність за вимогами");
            Console.Write("> ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddOfferToClient();
                    break;
                case "2":
                    RemoveOfferFromClient();
                    break;
                case "3":
                    CheckAvailability();
                    break;
            }
        }

        private void AddOfferToClient()
        {
            try
            {
                ShowAllClients();
                Console.Write("Введіть ID клієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int clientId))
                {
                    Console.WriteLine("Невірний ID.");
                    return;
                }

                ShowAllRealEstate();
                Console.Write("Введіть ID об'єкта нерухомості: ");
                if (!int.TryParse(Console.ReadLine(), out int realEstateId))
                {
                    Console.WriteLine("Невірний ID.");
                    return;
                }

                _offerService.AddOfferToClient(clientId, realEstateId);

                Console.WriteLine("Пропозицію успішно додано.");
            }
            catch (OfferLimitException ex) 
            {
                Console.WriteLine($"Помилка бізнес-логіки: {ex.Message}");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Помилка валідації: {ex.Message}");
            }
        }

        private void RemoveOfferFromClient()
        {
            try
            {
                ShowAllClients();

                Console.WriteLine("\n--- Видалення (відхилення) пропозиції ---");

                Console.Write("Введіть ID клієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int clientId))
                {
                    Console.WriteLine("Помилка: ID має бути числом.");
                    return;
                }

                Console.Write("Введіть ID об'єкта нерухомості, який треба видалити зі списку клієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int realEstateId))
                {
                    Console.WriteLine("Помилка: ID має бути числом.");
                    return;
                }

                _offerService.RemoveOfferFromClient(clientId, realEstateId);

                Console.WriteLine("Пропозицію успішно видалено (відхилено клієнтом).");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Помилка валідації: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка: {ex.Message}");
            }
        }

        private void CheckAvailability()
        {
            try
            {
                Console.WriteLine("\n--- Перевірка наявності за вимогами ---");

                Console.WriteLine("Оберіть бажаний тип об'єкта:");
                Console.WriteLine("0 - 1-кімнатна");
                Console.WriteLine("1 - 2-кімнатна");
                Console.WriteLine("2 - 3-кімнатна");
                Console.WriteLine("3 - Приватна ділянка");
                Console.Write("> ");

                if (!Enum.TryParse(Console.ReadLine(), out RealEstateType type) || !Enum.IsDefined(typeof(RealEstateType), type))
                {
                    Console.WriteLine("Невірний тип нерухомості.");
                    return;
                }

                Console.Write("Введіть максимальну прийнятну вартість: ");
                if (!double.TryParse(Console.ReadLine(), out double cost))
                {
                    Console.WriteLine("Вартість має бути числом.");
                    return;
                }

                bool isAvailable = _offerService.CheckAvailability(type, cost);

                if (isAvailable)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[Результат]: Так, об'єкт типу '{type}' за ціною до {cost} є в наявності!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Результат]: На жаль, об'єктів типу '{type}' за такою ціною зараз немає.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при перевірці: {ex.Message}");
            }
        }
    }
}
