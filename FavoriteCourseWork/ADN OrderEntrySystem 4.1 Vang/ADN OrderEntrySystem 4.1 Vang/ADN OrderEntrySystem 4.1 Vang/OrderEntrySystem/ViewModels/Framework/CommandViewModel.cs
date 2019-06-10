using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderEntrySystem
{
    /// <summary>
    /// The class representing a view model for commands.
    /// </summary>
    public class CommandViewModel : ViewModel
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="displayName">The name of the command.</param>
        /// <param name="command">The logic of the command</param>
        public CommandViewModel(string displayName, ICommand command)
            : this(displayName, command, false, false, string.Empty)
        {
        }

        public CommandViewModel(string displayName, ICommand command, bool isDefault, bool isCancel) : this(displayName, command, isDefault, isCancel, string.Empty)
        {
        }

        public CommandViewModel(string displayName, ICommand command, bool isDefault, bool isCancel, string groupName)
            : base(displayName)
        {
            if (command == null)
            {
                throw new Exception("Command was null.");
            }

            this.Command = command;
            this.IsCancel = isCancel;
            this.IsDefault = isDefault;
            this.GroupName = groupName;
        }

        public CommandViewModel(string displayName, ICommand command, string groupName)
            : this(displayName, command, false, false, groupName)
        {
        }

        /// <summary>
        /// Gets the command to be viewed.
        /// </summary>
        public ICommand Command { get; private set; }

        public bool IsDefault { get; private set; }

        public bool IsCancel { get; private set; }

        public string GroupName { get; private set; }
    }
}