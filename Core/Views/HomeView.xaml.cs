using Core.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace Core.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        HomeViewModel homeViewModel = new HomeViewModel(DialogCoordinator.Instance);

        public HomeView()
        {
            InitializeComponent();
            DataContext = homeViewModel;
        }
    }
}
