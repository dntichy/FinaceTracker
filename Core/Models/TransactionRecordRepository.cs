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
    class TransactionRecordRepository : IPersitable<TransactionRecord>
    {
        public List<TransactionRecord> TxRepository { get; set; }

        private const string TransactionsPath = "data.json";

        public TransactionRecordRepository()
        {
            TxRepository = GetTxRepository();
        }

        private List<TransactionRecord> GetTxRepository()
        {
            if (File.Exists(TransactionsPath))
            {
                var list = JsonConvert.DeserializeObject<List<TransactionRecord>>(File.ReadAllText(TransactionsPath));
                var list2 = new List<TransactionRecord>(list.OrderByDescending(item => item.Date));
                return list2;
            }
            else return null;
        }

        public void Add(TransactionRecord txRecord)
        {
            txRecord.Id = TxRepository.Max(n => n.Id) + 1;
            TxRepository.Add(txRecord);
            PersistRecords();

        }

        private void PersistRecords()
        {
            string json = JsonConvert.SerializeObject(TxRepository, Formatting.Indented);
            File.WriteAllText(TransactionsPath, json);
        }

        public void Remove(TransactionRecord record)
        {
            TxRepository.Remove(record);
            PersistRecords();
        }

        public void Modify(TransactionRecord record)
        {
            var itemToModify = TxRepository.Where(n=>n.Id==record.Id);
            if (itemToModify == null) throw new Exception("not found");
            var tx = (TransactionRecord)itemToModify;
            tx.Amount = record.Amount;
            tx.Category = record.Category;
            tx.Date = record.Date;
            tx.Shop = record.Shop;
            PersistRecords();
        }
    }
}
