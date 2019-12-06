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
    class ShopViewModel : INotifyPropertyChanged
    {
        private ShopRepository ShopRepository { get; set; }

        private ObservableCollection<Shop> shops;
        public ObservableCollection<Shop> Shops
        {
            get { return shops; }
            set
            {
                shops = value;
                OnPropertyChanged("Shops");
            }
        }
        public Category SelectedShop { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }
        public ShopViewModel()
        {
            ShopRepository = new ShopRepository();
            Shops = new ObservableCollection<Shop>(ShopRepository.Shops);
            ReorderShopList();
            AddNewCommand = new RelayCommand<object>(AddNewShop);
            RemoveCommand = new RelayCommand<object>(RemoveShop);
            EditCommand = new RelayCommand<object>(EditShop);
        }

        private void AddNewShop(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveShop(object obj)
        {
            throw new NotImplementedException();
        }

        private void EditShop(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReorderShopList()
        {
            var list = new ObservableCollection<Shop>(Shops.OrderByDescending(item => item.Id));
            Shops = list;
            Shops.CollectionChanged += OnListChanged;
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
