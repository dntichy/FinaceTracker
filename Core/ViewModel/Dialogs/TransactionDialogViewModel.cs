using Core.Models;
using Core.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Core.ViewModel.Dialogs
{
    class TransactionDialogViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Shop> ShoppingPlaceDistinct { get; set; }
        public Transaction TxRecord { get; set; }

        public bool Cancel { get; set; }

        public ICommand CloseTxAddDialogCommand { get; set; }
        public ICommand ConfirmTxAddDialogCommand { get; set; }


        public TransactionDialogViewModel(Action<TransactionDialogViewModel> closeHandler)
        {
            Cancel = false;
            var CategRepo = new CategoryRepository();
            var ShopRepo = new ShopRepository();
            ShoppingPlaceDistinct = new ObservableCollection<Shop>(ShopRepo.Shops);
            Categories = new ObservableCollection<Category>(CategRepo.Categories);

            TxRecord = new Transaction() { Date = DateTime.Now};
            
            CloseTxAddDialogCommand = new SimpleCommand
            {
                ExecuteDelegate = o =>
                {
                    Cancel = true;
                    closeHandler(this);
                }
            };
            ConfirmTxAddDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
        }
    }
}
