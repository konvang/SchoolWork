using System.ComponentModel;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class LocationViewModel : EntityViewModel<Location>, IDataErrorInfo
    {
 
        public LocationViewModel(Location location)
            : base("New location", location)
        {
            this.Entity = location;
            this.LocationRepository = (Repository<Location>)RepositoryManager.GetRepository(typeof(Location));

        }

        public Repository<Location> LocationRepository { get; set; }


        public string Error
        {
            get
            {
                return this.Entity.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.Entity[propertyName];
            }
        }

        public Location Location
        {
            get
            {
                return this.Entity;
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.TextBox, 1)]
        public string Name
        {
            get
            {
                return this.Entity.Name;
            }
            set
            {
                this.Entity.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        [EntityColumn(2, 75)]
        [EntityControl(ControlType.TextBox, 2)]
        public string Description
        {
            get
            {
                return this.Entity.Description;
            }
            set
            {
                this.Entity.Description = value;
                this.OnPropertyChanged("Description");
            }
        }

        [EntityColumn(3, 75)]
        [EntityControl(ControlType.TextBox, 3)]
        public string City
        {
            get
            {
                return this.Entity.City;
            }
            set
            {
                this.Entity.City = value;
                this.OnPropertyChanged("City");
            }
        }

        [EntityColumn(4, 75)]
        [EntityControl(ControlType.TextBox, 4)]
        public string State
        {
            get
            {
                return this.Entity.State;
            }
            set
            {
                this.Entity.State = value;
                this.OnPropertyChanged("State");
            }
        }

    }
}