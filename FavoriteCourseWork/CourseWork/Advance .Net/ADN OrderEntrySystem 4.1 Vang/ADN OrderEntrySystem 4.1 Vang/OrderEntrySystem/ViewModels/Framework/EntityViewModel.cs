using OrderEntryDataAccess;
using OrderEntryEngine;
using System.Windows.Input;

namespace OrderEntrySystem
{
    public class EntityViewModel<T> : WorkspaceViewModel 
        where T : class, IEntity
    {

        private bool isSelected;

        private ICommand ok, cancel;

        public EntityViewModel(string displayName, T entity)
            : base(displayName)
        {
            this.Entity = entity;
        }

        public T Entity { get; set; }

        protected override void CreateCommands()
        {
            ////this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            ////this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
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

        public void Save()
        {
            Repository<T> repository = RepositoryManager.GetRepository(typeof(T)) as Repository<T>;
            repository.AddEntity(this.Entity);
            repository.SaveToDatabase();
        }

        [EntityControl(ControlType.Button, 8)]
        public ICommand Ok
        {
            get
            {
                return this.ok ?? (this.ok = new DelegateCommand(p => this.OkExecute()));
            }
        }

        [EntityControl(ControlType.Button, 9)]
        public ICommand Cancel
        {
            get
            {
                return this.cancel ?? (this.cancel = new DelegateCommand(p => this.CancelExecute()));
            }
        }

        private void OkExecute()
        {
            this.Save();
          
            this.CloseAction(true);
        }

        /// <summary>
        /// Closes the window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }

    }
}