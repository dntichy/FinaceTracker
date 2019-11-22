using Core.Models;
using Core.ViewModel.Dialogs;
using Core.Views;
using Core.Views.Dialogs;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Core.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        private TransactionRecordRepository TransactionRecordRepository { get; set; }
        private IDialogCoordinator dialogCoordinator;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<TransactionRecord> transactions;
        public ObservableCollection<TransactionRecord> Transactions
        {
            get { return transactions; }
            set
            {
                transactions = value;
                OnPropertyChanged("Transactions");
            }
        }
        public TransactionRecord SelectedTransaction { get; set; }

        public ICommand AddNewTxCommand { get; set; }

        public ICommand RemoveTxCommand { get; set; }

        public ICommand EditTxCommand { get; set; }

        public HomeViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;
            TransactionRecordRepository = new TransactionRecordRepository();
            transactions = new ObservableCollection<TransactionRecord>(TransactionRecordRepository.TxRepository);

            AddNewTxCommand = new RelayCommand<object>(AddNewTx);
            RemoveTxCommand = new RelayCommand<object>(RemoveTx);
            EditTxCommand = new RelayCommand<object>(EditTx);
        }

      

        private void EditTx(object obj)
        {
        }

        private void RemoveTx(object obj)
        {

            if (SelectedTransaction != null)
            {
                Transactions.Remove(SelectedTransaction);
                ReorderTransactionList();
            }

        }

        private void AddNewTx(object obj)
        {
            GetUserInputAsync();

        }

        private async Task GetUserInputAsync()
        {
            
            var custom_dialog = new AddTransactionDialog();
            var dialogViewModel = new AddTransactionDialogViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, custom_dialog);
                if (!instance.Cancel) ProcessUserInput(instance.TxRecord);
            });
            custom_dialog.DataContext = dialogViewModel;

            await dialogCoordinator.ShowMetroDialogAsync(this, custom_dialog);
        }

        public void ProcessUserInput(TransactionRecord txRecord)
        {
            Transactions.Add(txRecord);
            ReorderTransactionList(); // todo hook onchange event => persist  and  reorder
        }

        private void ReorderTransactionList()
        {
            var list = new ObservableCollection<TransactionRecord>(Transactions.OrderByDescending(item => item.Date));
            Transactions = list;

        }
    }
}
