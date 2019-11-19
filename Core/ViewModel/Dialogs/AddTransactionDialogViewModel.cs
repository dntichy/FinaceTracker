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
        private const string CategoriesPath = "categories.json";

        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<string> ShoppingPlaceDistinct { get; set; }
        public TransactionRecord TxRecord { get; set; }

        public bool Cancel { get; set; }

        public ICommand CloseTxAddDialogCommand { get; set; }
        public ICommand ConfirmTxAddDialogCommand { get; set; }


        public AddTransactionDialogViewModel(Action<AddTransactionDialogViewModel> closeHandler)
        {
            LoadCategoriesFromJson();
            TxRecord = new TransactionRecord() { Category = new Category() };
            Cancel = false;
            CloseTxAddDialogCommand = new SimpleCommand
            {
                ExecuteDelegate = o =>
                {
                    this.Cancel = true;
                    closeHandler(this);
                }
            };
            ConfirmTxAddDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
        }

        private void LoadCategoriesFromJson()
        {
            if (File.Exists(CategoriesPath))
            {
                var list = JsonConvert.DeserializeObject<ObservableCollection<Category>>(File.ReadAllText(CategoriesPath));
                Categories = list;
            }
        }
    }
}
