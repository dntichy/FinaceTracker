using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.Models.Repositories
{
    class CategoryViewModel : INotifyPropertyChanged
    {
        private CategoryRepository CategoryRepository { get; set; }

        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }
        public Category SelectedCategory { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        public CategoryViewModel()
        {
            CategoryRepository = new CategoryRepository();
            Categories = new ObservableCollection<Category>(CategoryRepository.Categories);
            ReorderCategoryList();
            AddNewCommand = new RelayCommand<object>(AddNewCategory);
            RemoveCommand = new RelayCommand<object>(RemoveCategory);
            EditCommand = new RelayCommand<object>(EditCategory);
        }

        private void AddNewCategory(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveCategory(object obj)
        {
            throw new NotImplementedException();
        }

        private void EditCategory(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReorderCategoryList()
        {
            var list = new ObservableCollection<Category>(Categories.OrderByDescending(item => item.Id));
            Categories = list;
            Categories.CollectionChanged += OnListChanged;
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
