using Core.Models;
using System;
using System.ComponentModel;

namespace Core.Views
{
    public class TransactionRecord : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public float Amount { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ShoppingPlace { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}