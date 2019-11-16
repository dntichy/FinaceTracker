using Core.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class HomeViewModel
    {

        private ObservableCollection<TransactionRecord> transactions;
        public ObservableCollection<TransactionRecord> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }

        public ICommand AddNewTxCommand { get; set; }

        public ICommand RemoveTxCommand { get; set; }

        public ICommand EditTxCommand { get; set; }

        public HomeViewModel()
        {
            transactions = new ObservableCollection<TransactionRecord>();
            LoadFromJsonFile();

            AddNewTxCommand = new RelayCommand<object>(AddNewTx);
            RemoveTxCommand = new RelayCommand<object>(RemoveTx);
            EditTxCommand = new RelayCommand<object>(EditTx);
        }

        private void EditTx(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveTx(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddNewTx(object obj)
        {
            MessageBox.Show("Add new Tx");
            //throw new NotImplementedException();
        }

        public void ImportFromCSV(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show("File doesn't exist");
            }
            else
            {
                var linesOfDocument = File.ReadAllLines(fileName, Encoding.GetEncoding("windows-1250"));
                bool isFirstLine = true;
                foreach (var line in linesOfDocument)
                {

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    var splitLine = line.Split(';');
                    var date = splitLine[0];
                    var amount = splitLine[1];
                    var shopPlace = splitLine[2];
                    var category = splitLine[3];

                    if (date == "" || amount == "") return;

                    var tx = new TransactionRecord()
                    {
                        Amount = float.Parse(amount),
                        Category = category,
                        Date = DateTime.Parse(date),
                        Description = "",
                        ShoppingPlace = shopPlace
                    };
                    Transactions.Add(tx);
                }
            }
        }

        private void ImportFromCSVMenuItemClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse CSV Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                var filePath = openFileDialog1.FileName;
                ImportFromCSV(filePath);
            }
        }

        private void SaveRecordsToJsonFile(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(Transactions);
            File.WriteAllText("data.dat", json);
        }

        private void LoadFromJsonFile()
        {
            if (File.Exists("data.dat"))
            {
                var list = JsonConvert.DeserializeObject<ObservableCollection<TransactionRecord>>(File.ReadAllText("data.dat"));
                transactions = list;
            }
        }
    }
}
