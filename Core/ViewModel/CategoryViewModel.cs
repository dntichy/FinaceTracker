﻿using Core.ViewModel.Dialogs;
using Core.Views.Dialogs;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.Models.Repositories
{
    class CategoryViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;
        private CategoryRepository CategoryRepository { get; set; }

        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }
        public Category SelectedCategory { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(name));
        }

        public CategoryViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;
            CategoryRepository = new CategoryRepository();
            Categories = new ObservableCollection<Category>(CategoryRepository.Categories);
            ReorderCategoryList();
            AddNewCommand = new RelayCommand<object>(AddNewCategory);
            RemoveCommand = new RelayCommand<object>(RemoveCategory);
            EditCommand = new RelayCommand<object>(EditCategory);
        }

        private void AddNewCategory(object obj)
        {
            GetUserInputAsync();
        }
        private async Task GetUserInputAsync()
        {

            var categoryDialog = new CategoryDialog();
            var dialogViewModel = new CategoryDialogViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, categoryDialog);
                if (!instance.Cancel) ProcessUserInput(instance.Category);
            });
            categoryDialog.DataContext = dialogViewModel;

            await dialogCoordinator.ShowMetroDialogAsync(this, categoryDialog);
        }

        public void ProcessUserInput(Category category)
        {
            Categories.Add(category);
        }

        private void RemoveCategory(object obj)
        {
            throw new NotImplementedException();
        }

        private void EditCategory(object obj)
        {
            throw new NotImplementedException();
        }

        private void ReorderCategoryList()
        {
            var list = new ObservableCollection<Category>(Categories.OrderByDescending(item => item.Id));
            Categories = list;
            Categories.CollectionChanged += OnListChanged;
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
