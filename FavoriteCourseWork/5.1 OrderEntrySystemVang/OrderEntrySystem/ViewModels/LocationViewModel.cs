using System.ComponentModel;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class LocationViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private Location location;

        private Repository repository;

        private bool isSelected;

        public LocationViewModel(Location location, Repository repository)
            : base("New location")
        {
            this.location = location;
            this.repository = repository;
        }

        public string Error
        {
            get
            {
                return this.location.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.location[propertyName];
            }
        }

        public Location Location
        {
            get
            {
                return this.location;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public string Name
        {
            get
            {
                return this.location.Name;
            }
            set
            {
                this.location.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get
            {
                return this.location.Description;
            }
            set
            {
                this.location.Description = value;
                this.OnPropertyChanged("Description");
            }
        }

        public string City
        {
            get
            {
                return this.location.City;
            }
            set
            {
                this.location.City = value;
                this.OnPropertyChanged("City");
            }
        }

        public string State
        {
            get
            {
                return this.location.State;
            }
            set
            {
                this.location.State = value;
                this.OnPropertyChanged("State");
            }
        }

        /// <summary>
        /// Creates the commands needed for the car view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        private bool Save()
        {
            bool result = true;

            if (this.Location.IsValid)
            {
                // Add location to repository.
                this.repository.AddLocation(this.location);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("One or more properties are invalid. Location could not be saved.");
                result = false;
            }

            return result;
        }

        private void OkExecute()
        {
            if (this.Save())
            {
                this.CloseAction(true);
            }
        }

        /// <summary>
        /// Closes the new car window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}