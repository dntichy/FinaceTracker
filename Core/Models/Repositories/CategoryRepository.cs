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
    public class CategoryRepository : IPersitable<Category>
    {
        public ObservableCollection<Category> Categories { get; }
        private const string CategoryPath = "Categories.json";


        public CategoryRepository()
        {
            Categories = new ObservableCollection<Category>(GetCategoryRepository());
            Categories.CollectionChanged += OnListChanged;
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

        private ObservableCollection<Category> GetCategoryRepository()
        {
            if (File.Exists(CategoryPath))
            {
                return JsonConvert.DeserializeObject<ObservableCollection<Category>>(File.ReadAllText(CategoryPath));
            }
            else return new ObservableCollection<Category>();
        }
        private void PersistRecords()
        {
            string json = JsonConvert.SerializeObject(Categories, Formatting.Indented);
            File.WriteAllText(CategoryPath, json);
        }

        public void Add(Category record)
        {
            record.Id = Categories.Max(n => n.Id) + 1;
            Categories.Add(record);
        }

        public void Modify(Category record)
        {
            var recordToModify = Categories.Where(n => n.Name == record.Name).First();
            if (recordToModify != null) recordToModify = record;
        }

        public void Remove(Category record)
        {
            Categories.Remove(record);
        }
    }
}
