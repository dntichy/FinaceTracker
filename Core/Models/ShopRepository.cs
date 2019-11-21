using Core.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    class ShopRepository : IPersitable<Shop>
    {
        public List<Shop> ShRepository { get; set; }

        private const string TransactionsPath = "ShoppingPlaces.json";

        public ShopRepository()
        {
            ShRepository = GetShopRepository();
        }

        private List<Shop> GetShopRepository()
        {
            if (File.Exists(TransactionsPath))
            {
                var list = JsonConvert.DeserializeObject<List<Shop>>(File.ReadAllText(TransactionsPath));
                return list;
            }
            else return null;
        }

       

        private void PersistRecords()
        {
            string json = JsonConvert.SerializeObject(ShRepository, Formatting.Indented);
            File.WriteAllText(TransactionsPath, json);
        }

      

        public void Add(Shop record)
        {
            throw new NotImplementedException();
        }

        public void Remove(Shop record)
        {
            throw new NotImplementedException();
        }

        public void Modify(Shop record)
        {
            throw new NotImplementedException();
        }
    }
}
