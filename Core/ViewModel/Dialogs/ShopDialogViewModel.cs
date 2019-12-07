using Core.Models;
using Core.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Core.ViewModel.Dialogs
{
    class ShopDialogViewModel
    {
        public ObservableCollection<Shop> Shops { get; set; }
        public Shop Shop { get; set; }
        public bool Cancel { get; set; }

        public ICommand CloseDialogCommand { get; set; }
        public ICommand ConfirmDialogCommand { get; set; }

        public ShopDialogViewModel(Action<ShopDialogViewModel> closeHandler)
        {
            Cancel = false;
            var ShopRepo = new ShopRepository();
            Shops = new ObservableCollection<Shop>(ShopRepo.Shops);

            CloseDialogCommand = new SimpleCommand
            {
                ExecuteDelegate = o =>
                {
                    Cancel = true;
                    closeHandler(this);
                }
            };
            ConfirmDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
        }
    }
}
