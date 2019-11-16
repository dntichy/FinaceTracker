using System;
using System.ComponentModel;

namespace Core.Views
{
    public class TransactionRecord : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ShoppingPlace { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}