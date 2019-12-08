using Core.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace Core.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        TransactionViewModel transactionViewModel = new TransactionViewModel(DialogCoordinator.Instance);

        public TransactionView()
        {
            InitializeComponent();
            DataContext = transactionViewModel;
        }
    }
}
