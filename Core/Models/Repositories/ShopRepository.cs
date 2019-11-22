using Core.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    class ShopRepository : IPersitable<Shop>
    {
        public ObservableCollection<Shop> Shops { get; set; }
        private const string TransactionsPath = "ShoppingPlaces.json";

        public ShopRepository()
        {
            Shops = GetShopRepository();
            Shops.CollectionChanged += OnListChanged;
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    PersistRecords();
                    break;
            }
        }

        private ObservableCollection<Shop> GetShopRepository()
        {
            if (File.Exists(TransactionsPath))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<Shop>>(File.ReadAllText(TransactionsPath));
            }
            else return new ObservableCollection<Shop>();
        }

        private void PersistRecords()
        {
            string json = JsonConvert.SerializeObject(Shops, Formatting.Indented);
            File.WriteAllText(TransactionsPath, json);
        }

        public void Add(Shop record)
        {
            Shops.Add(record);
            PersistRecords();
        }

        public void Remove(Shop record)
        {
            Shops.Remove(record);
            PersistRecords();
        }

        public void Modify(Shop record)
        {
            var shopToModify = Shops.Where(n => n.ShopName == record.ShopName).First();
            shopToModify = record;
            PersistRecords();
        }
    }
}
