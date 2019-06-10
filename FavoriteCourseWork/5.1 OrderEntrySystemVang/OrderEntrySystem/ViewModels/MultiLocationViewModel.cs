using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiLocationViewModel : WorkspaceViewModel
    {
        private Repository repository;

        public MultiLocationViewModel(Repository repository)
            : base("All locations")
        {
            this.repository = repository;

            List<LocationViewModel> locations =
                (from location in this.repository.GetLocations()
                select new LocationViewModel(location, this.repository)).ToList();

            locations.ForEach(lvm => lvm.PropertyChanged += this.OnLocationViewModelPropertyChanged);

            this.AllLocations = new ObservableCollection<LocationViewModel>(locations);

            this.repository.LocationAdded += this.OnLocationAdded;
            this.repository.LocationRemoved += this.OnLocationRemoved;
        }

        public ObservableCollection<LocationViewModel> AllLocations { get; private set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllLocations.Count(vm => vm.IsSelected);
            }
        }

        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewLocationExecute())));
            this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditLocationExecute(), p => this.NumberOfItemsSelected == 1)));
            this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteLocationExecute(), p => this.NumberOfItemsSelected == 1)));
        }

        private void OnLocationAdded(object sender, LocationEventArgs e)
        {
            LocationViewModel vm = new LocationViewModel(e.Location, this.repository);
            vm.PropertyChanged += this.OnLocationViewModelPropertyChanged;

            this.AllLocations.Add(vm);
        }

        /// <summary>
        /// A handler which responds when a location view model's property changes.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLocationViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewLocationExecute()
        {
            Location location = new Location();

            LocationViewModel viewModel = new LocationViewModel(location, this.repository);

            this.ShowLocation(viewModel);
        }

        private void EditLocationExecute()
        {
            LocationViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowLocation(viewModel);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select only one car");
            }
        }

        private void DeleteLocationExecute()
        {
            LocationViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected location?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveLocation(viewModel.Location);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one location");
            }
        }

        private LocationViewModel GetOnlySelectedViewModel()
        {
            LocationViewModel result;

            try
            {
                result = this.AllLocations.Single(vm => vm.IsSelected);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates a new window to edit a car.
        /// </summary>
        /// <param name="viewModel">The view model for the car to be edited.</param>
        private void ShowLocation(LocationViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            LocationView view = new LocationView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }

        private void OnLocationRemoved(object sender, LocationEventArgs e)
        {
            LocationViewModel viewModel = this.GetOnlySelectedViewModel();
            if (viewModel != null)
            {
                if (viewModel.Location == e.Location)
                {
                    this.AllLocations.Remove(viewModel);
                }
            }
        }
    }
}