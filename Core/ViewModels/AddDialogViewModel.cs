using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;

namespace Core.ViewModels
{
    public class AddDialogViewModel
    {

        public ICommand CloseAddDialogCommand { get; private set; }

        public AddDialogViewModel()
        {
            CloseAddDialogCommand = new RelayCommand<IClosable>(CloseAddDialog);
        }

        private void CloseAddDialog(IClosable obj)
        {
            if(obj!= null)
                obj.Close();
        }
    }
}
