using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RealtorFirm.DAL.Entities;
using RealtorFirm.DAL.Interfaces;
using System.IO;

namespace RealtorFirm.DAL.Repositories
{
    public class FileRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private List<T> _items;

        public FileRepository(string filePath)
        {
            _filePath = filePath;
            _items = LoadFromFile();
        }

        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void SaveChanges()
        {
            string json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void Create(T item)
        {
            item.Id = (_items.Any() ? _items.Max(i => i.Id) : 0) + 1;
            _items.Add(item);
        }

        public void Delete(int id)
        {
            var item = Get(id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public T Get(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public void Update(T item)
        {
            var existingItem = Get(item.Id);
            if (existingItem != null)
            {
                var index = _items.IndexOf(existingItem);
                    _items[index] = item;
            }
        }
    }
}
