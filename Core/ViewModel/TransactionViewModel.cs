﻿using Core.Models;
using Core.ViewModel.Dialogs;
using Core.Views;
using Core.Views.Dialogs;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Core.ViewModels
{
    public class TransactionViewModel : INotifyPropertyChanged
    {

        private TransactionRecordRepository TransactionRecordRepository { get; set; }
        private IDialogCoordinator dialogCoordinator;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Transaction> transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get { return transactions; }
            set
            {
                transactions = value;
                OnPropertyChanged("Transactions");
            }
        }
        public Transaction SelectedTransaction { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public TransactionViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;
            TransactionRecordRepository = new TransactionRecordRepository();
            Transactions = new ObservableCollection<Transaction>(TransactionRecordRepository.Transactions);
            ReorderTransactionList();
            AddNewCommand = new RelayCommand<object>(AddNewTx);
            RemoveCommand = new RelayCommand<object>(RemoveTxAsync);
            EditCommand = new RelayCommand<object>(EditTx);
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Transaction item in e.NewItems)
                        TransactionRecordRepository.Add(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Transaction item in e.OldItems)
                        TransactionRecordRepository.Remove(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
            }
            ReorderTransactionList();
        }

        private void EditTx(object obj)
        {
        }

        private async void RemoveTxAsync(object obj)
        {
            if (SelectedTransaction != null)
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
               var result = await metroWindow.ShowMessageAsync("Are you sure", "Are you sure you want to delete? ", MessageDialogStyle.AffirmativeAndNegative);
               if(result == MessageDialogResult.Affirmative)
                {
                    Transactions.Remove(SelectedTransaction);
                }
            }
        }

        private void AddNewTx(object obj)
        {
            GetUserInputAsync();

        }

        private async Task GetUserInputAsync()
        {

            var txDialog = new TransactionDialog();
            var dialogViewModel = new TransactionDialogViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, txDialog);
                if (!instance.Cancel) ProcessUserInput(instance.TxRecord);
            });
            txDialog.DataContext = dialogViewModel;

            await dialogCoordinator.ShowMetroDialogAsync(this, txDialog);
        }

        public void ProcessUserInput(Transaction txRecord)
        {
            Transactions.Add(txRecord);
        }

        private void ReorderTransactionList()
        {
            var list = new ObservableCollection<Transaction>(Transactions.OrderByDescending(item => item.Date));
            Transactions = list;
            Transactions.CollectionChanged += OnListChanged;

        }
    }
}
