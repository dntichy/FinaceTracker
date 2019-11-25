using Core.Models;
using Core.Views;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Core.ViewModel.Dialogs
{
    class AddTransactionDialogViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Shop> ShoppingPlaceDistinct { get; set; }
        public TransactionRecord TxRecord { get; set; }

        public bool Cancel { get; set; }

        public ICommand CloseTxAddDialogCommand { get; set; }
        public ICommand ConfirmTxAddDialogCommand { get; set; }


        public AddTransactionDialogViewModel(Action<AddTransactionDialogViewModel> closeHandler)
        {
            Cancel = false;
            var CategRepo = new CategoryRepository();
            var ShopRepo = new ShopRepository();
            ShoppingPlaceDistinct = new ObservableCollection<Shop>(ShopRepo.Shops);
            Categories = new ObservableCollection<Category>(CategRepo.Categories);

            TxRecord = new TransactionRecord() { Date = DateTime.Now};
            
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
