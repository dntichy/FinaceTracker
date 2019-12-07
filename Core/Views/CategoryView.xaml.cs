using Core.Models.Repositories;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace Core.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        CategoryViewModel categoryViewModel = new CategoryViewModel(DialogCoordinator.Instance);
        public CategoryView()
        {
            InitializeComponent();
            DataContext = categoryViewModel;
        }
    }
}
