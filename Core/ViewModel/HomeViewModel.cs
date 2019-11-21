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
        //private const string TransactionsPath = "data.json";

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
                //SaveRecordsToJsonFile();
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
            TransactionRecordRepository.Add(txRecord);
            ReorderTransactionList();
        }

        private void ReorderTransactionList()
        {
            var list = new ObservableCollection<TransactionRecord>(Transactions.OrderByDescending(item => item.Date));
            Transactions = list;

        }

        //public void ImportFromCSV(string fileName)
        //{
        //    if (!File.Exists(fileName))
        //    {
        //        MessageBox.Show("File doesn't exist");
        //    }
        //    else
        //    {
        //        var linesOfDocument = File.ReadAllLines(fileName, Encoding.GetEncoding("windows-1250"));
        //        bool isFirstLine = true;
        //        foreach (var line in linesOfDocument)
        //        {

        //            if (isFirstLine)
        //            {
        //                isFirstLine = false;
        //                continue;
        //            }
        //            var splitLine = line.Split(';');
        //            var date = splitLine[0];
        //            var amount = splitLine[1];
        //            var shopPlace = splitLine[2];
        //            var category = splitLine[3];

        //            if (date == "" || amount == "") return;

        //            var tx = new TransactionRecord()
        //            {
        //                Amount = float.Parse(amount),
        //                Category = category,
        //                Date = DateTime.Parse(date),
        //                Description = "",
        //                ShoppingPlace = shopPlace
        //            };
        //            Transactions.Add(tx);
        //        }
        //    }
        //}

        //private void ImportFromCSVMenuItemClicked(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog
        //    {
        //        Title = "Browse CSV Files",

        //        CheckFileExists = true,
        //        CheckPathExists = true,

        //        DefaultExt = "csv",
        //        Filter = "csv files (*.csv)|*.csv",
        //        FilterIndex = 2,
        //        RestoreDirectory = true,

        //        ReadOnlyChecked = true,
        //        ShowReadOnly = true
        //    };

        //    if (openFileDialog1.ShowDialog() == true)
        //    {
        //        var filePath = openFileDialog1.FileName;
        //        //ImportFromCSV(filePath);
        //    }
        //}

        //private void SaveRecordsToJsonFile()
        //{
        //    string json = JsonConvert.SerializeObject(Transactions, Formatting.Indented);
        //    File.WriteAllText("data.json", json);
        //}


        //private void LoadTransactionsFromJson()
        //{
        //    if (File.Exists(TransactionsPath))
        //    {
        //        var list = JsonConvert.DeserializeObject<ObservableCollection<TransactionRecord>>(File.ReadAllText(TransactionsPath));
        //        var list2 = new ObservableCollection<TransactionRecord>(list.OrderByDescending(item => item.Date));
        //        Transactions = list2;
        //    }

        //}
    }
}
