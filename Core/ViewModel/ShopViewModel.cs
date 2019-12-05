using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Category> shops;
        public ObservableCollection<Category> Shops
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
    }
}
