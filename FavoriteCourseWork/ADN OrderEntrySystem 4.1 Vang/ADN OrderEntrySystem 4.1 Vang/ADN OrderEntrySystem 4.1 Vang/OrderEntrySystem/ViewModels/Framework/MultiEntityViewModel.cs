using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiEntityViewModel<TEntity, TViewModel, TView> : WorkspaceViewModel, IMultiEntityViewModel 
        where TEntity : class, IEntity 
        where TViewModel : EntityViewModel<TEntity> 
        where TView : UserControl
    {
        private Repository<TEntity> repository;

        public MultiEntityViewModel()
            : base("All " + typeof(TEntity).Name + "s")
        {
            this.repository = (Repository<TEntity>)RepositoryManager.GetRepository(typeof(TEntity));
            this.CreateAllViewModels();
            this.repository.EntityAdded += this.OnEntityAdded;
            this.repository.EntityRemoved += this.OnEntityRemove;
        }

        public ObservableCollection<TViewModel> AllEntities { get; set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllEntities.Count(vm => vm.IsSelected);
            }
        }

        public void AddPropertyChangedEvent(List<TViewModel> entities)
        {
            entities.ForEach(cvm => cvm.PropertyChanged += this.OnEntityViewModelPropertyChanged);
        }

        public Type Type
        {
            get
            {
                return typeof(TViewModel);
            }
        }

        public string DisplayName { get; }

        public EventHandler RequestClose()
        {
            return null;
        }

        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(parm => this.CreateNewEntityExecute())));
            this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(parm => this.EditEntityExecute(), p => this.NumberOfItemsSelected == 1)));
            this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(parm => this.DeleteEntityExecute(), p => this.NumberOfItemsSelected == 1)));
        }

        private void CreateAllViewModels()
        {
            List<TViewModel> entities =
                (from e in this.repository.GetEntities()
                 select Activator.CreateInstance(typeof(TViewModel), e) as TViewModel).ToList();

            entities.ForEach(en => en.PropertyChanged += this.OnEntityViewModelPropertyChanged);

            this.AllEntities = new ObservableCollection<TViewModel>(entities);
        }

        private void CreateNewEntityExecute()
        {
            IEntity entity = Activator.CreateInstance(typeof(TEntity)) as IEntity;

            TViewModel viewModel = Activator.CreateInstance(typeof(TViewModel), entity) as TViewModel;

            this.ShowEntity(viewModel as TViewModel);
        }

        private void EditEntityExecute()
        {
            TViewModel viewModel = this.AllEntities.Single(vm => vm.IsSelected);

            if(viewModel != null)
            {
                this.ShowEntity(viewModel);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select one item.");
            }
        }

        private void DeleteEntityExecute()
        {
            EntityViewModel<TEntity> viewModel = this.AllEntities.Single(vm => vm.IsSelected);

            if(viewModel != null)
            {
                if (MessageBox.Show("Do you want to delete the selected item?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveEntity(viewModel.Entity);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one item.");
            }
        }

        private void ShowEntity(TViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            TView view = Activator.CreateInstance(typeof(TView)) as TView;

            view.DataContext = viewModel;
            window.Content = view;
            window.ShowDialog();
        }

        private void OnEntityAdded(object sender, EntityEventArgs<TEntity> e)
        {
            TViewModel viewModel = Activator.CreateInstance(typeof(TViewModel), e.Entity) as TViewModel;

            viewModel.PropertyChanged += this.OnEntityViewModelPropertyChanged;

            this.AllEntities.Add(viewModel);
        }

        private void OnEntityRemove(object sender, EntityEventArgs<TEntity> e)
        {
            TViewModel viewModel = this.AllEntities.Single(vm => vm.IsSelected);

            if (viewModel != null)
            {
                if(viewModel.Entity == e.Entity)
                {
                    this.AllEntities.Remove(viewModel);
                }
            }
        }

        private void OnEntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if(e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }
    }
}
