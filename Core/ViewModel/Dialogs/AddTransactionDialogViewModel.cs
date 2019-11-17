using Core.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.ViewModel.Dialogs
{
    class AddTransactionDialogViewModel
    {
        public string UserInput { get; set; }   // User input returned

        public TransactionRecord TxRecord { get; set; }

        public bool Cancel { get; set; }  // Flagged true if user clicks cancel button
        public ICommand CloseTxAddDialogCommand { get; set; }
        public ICommand ConfirmTxAddDialogCommand { get; set; }


        public AddTransactionDialogViewModel(Action<AddTransactionDialogViewModel> closeHandler)
        {
            TxRecord = new TransactionRecord();
            Cancel = false;
            CloseTxAddDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
            ConfirmTxAddDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };

        }
    }
}
