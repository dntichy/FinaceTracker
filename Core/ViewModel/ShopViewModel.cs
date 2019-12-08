using Core.ViewModel.Dialogs;
using Core.Views.Dialogs;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Core.Models.Repositories
{
    class ShopViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;
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
        public Shop SelectedShop { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }
        public ShopViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;
            ShopRepository = new ShopRepository();
            Shops = new ObservableCollection<Shop>(ShopRepository.Shops);
            ReorderShopList();
            AddNewCommand = new RelayCommand<object>(AddNewShop);
            RemoveCommand = new RelayCommand<object>(RemoveShop);
            EditCommand = new RelayCommand<object>(EditShop);
        }

        private void AddNewShop(object obj)
        {
            GetUserInputAsync();
        }
        private async Task GetUserInputAsync()
        {

            var categoryDialog = new ShopDialog();
            var dialogViewModel = new ShopDialogViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, categoryDialog);
                if (!instance.Cancel) ProcessUserInput(instance.Shop);
            });
            categoryDialog.DataContext = dialogViewModel;

            await dialogCoordinator.ShowMetroDialogAsync(this, categoryDialog);
        }

        public void ProcessUserInput(Shop shop)
        {
            Shops.Add(shop);
        }

        private async void RemoveShop(object obj)
        {
            if (SelectedShop != null)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                var result = await metroWindow.ShowMessageAsync("Are you sure", "Are you sure you want to delete? ", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    Shops.Remove(SelectedShop);
                }
            }
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
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Shop item in e.NewItems)
                        ShopRepository.Add(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Shop item in e.OldItems)
                        ShopRepository.Remove(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
            }
            ReorderShopList();
        }
    }
}
