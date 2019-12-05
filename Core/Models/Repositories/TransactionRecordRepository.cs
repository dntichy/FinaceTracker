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
    class TransactionRecordRepository : IPersitable<Transaction>
    {
        public ObservableCollection<Transaction> TxRepository { get; }

        private const string TransactionsPath = "data.json";

        public TransactionRecordRepository()
        {
            TxRepository = GetTxRepository();
            TxRepository.CollectionChanged += OnListChanged;
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

        private ObservableCollection<Transaction> GetTxRepository()
        {
            if (File.Exists(TransactionsPath))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<Transaction>>(File.ReadAllText(TransactionsPath));
            }
            else return new ObservableCollection<Transaction>();
        }

        public void Add(Transaction txRecord)
        {
            txRecord.Id = TxRepository.Max(n => n.Id) + 1;
            TxRepository.Add(txRecord);
        }

        private void PersistRecords()
        {
            string json = JsonConvert.SerializeObject(TxRepository, Formatting.Indented);
            File.WriteAllText(TransactionsPath, json);
        }

        public void Remove(Transaction record)
        {

            TxRepository.Remove(record);
        }

        public void Modify(Transaction record)
        {
            var itemToModify = TxRepository.Where(n => n.Id == record.Id).First();
            if (itemToModify == null) throw new Exception("not found");
            itemToModify = record;
        }
    }
}
