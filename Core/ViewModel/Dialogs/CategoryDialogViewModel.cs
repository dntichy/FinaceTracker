using Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Core.ViewModel.Dialogs
{
    class CategoryDialogViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public Category Category { get; set; }
        public bool Cancel { get; set; }

        public ICommand CloseDialogCommand { get; set; }
        public ICommand ConfirmDialogCommand { get; set; }


        public CategoryDialogViewModel(Action<CategoryDialogViewModel> closeHandler)
        {
            Cancel = false;
            var CategRepo = new CategoryRepository();
            Categories = new ObservableCollection<Category>(CategRepo.Categories);
            Category = new Category();
            CloseDialogCommand = new SimpleCommand
            {
                ExecuteDelegate = o =>
                {
                    Cancel = true;
                    closeHandler(this);
                }
            };
            ConfirmDialogCommand = new SimpleCommand { ExecuteDelegate = o => closeHandler(this) };
        }
    }
}
