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
        public bool Cancel { get; set; }  // Flagged true if user clicks cancel button
        public string MessageText { get; set; }  // Message displayed to user
        public ICommand _closeCommand { get; set; }


        public AddTransactionDialogViewModel(Action<AddTransactionDialogViewModel> closeHandler)
        {
            Cancel = false;
            _closeCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
        }
    }
}
