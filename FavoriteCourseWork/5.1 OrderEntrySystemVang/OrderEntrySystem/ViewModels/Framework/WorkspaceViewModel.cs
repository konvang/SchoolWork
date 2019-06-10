using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderEntrySystem
{
    public abstract class WorkspaceViewModel : ViewModel
    {
        /// <summary>
        /// A command which will close the workspace.
        /// </summary>
        private DelegateCommand closeCommand;

        /// <summary>
        /// A collection of commands for the workspace.
        /// </summary>
        private ObservableCollection<CommandViewModel> commands = new ObservableCollection<CommandViewModel>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="displayName">The name of the view model.</param>
        public WorkspaceViewModel(string displayName)
            : base(displayName)
        {
            this.CreateCommands();
        }

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        public Action<bool> CloseAction { get; set; }

        /// <summary>
        /// Gets the command that -- when invoked -- attempts to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                // Use lazy instantiation.
                if (this.closeCommand == null)
                {
                    this.closeCommand = new DelegateCommand(p => this.OnRequestClose());
                }

                return this.closeCommand;
            }
        }

        /// <summary>
        /// Gets a read-only list of commands that the UI can display and execute.
        /// </summary>
        public ObservableCollection<CommandViewModel> Commands
        {
            get
            {
                return this.commands;
            }
        }

        /// <summary>
        /// Creates commands for the view model. Descendants may (optionally) add commands.
        /// </summary>
        protected abstract void CreateCommands();

        /// <summary>
        /// A handler which responds when a request to close has been made.
        /// </summary>
        private void OnRequestClose()
        {
            // If "close" event handler is assigned...
            if (this.RequestClose != null)
            {
                // Call event handler, passing self in.
                this.RequestClose(this, EventArgs.Empty);
            }
        }
    }
}