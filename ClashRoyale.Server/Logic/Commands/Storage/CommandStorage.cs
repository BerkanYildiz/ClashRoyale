namespace ClashRoyale.Server.Logic.Commands.Storage
{
    using System;
    using System.Collections.Generic;

    public class CommandStorage : IDisposable
    {
        internal List<Command> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandStorage"/> class.
        /// </summary>
        public CommandStorage()
        {
            this.Commands = new List<Command>(32);
        }

        /// <summary>
        /// Adds the specified command.
        /// </summary>
        internal void AddCommand(Command Command)
        {
            this.Commands.Add(Command);
        }

        /// <summary>
        /// Removes all commands.
        /// </summary>
        internal void RemoveCommands()
        {
            this.Commands.Clear();
        }

        /// <summary>
        /// Convertes this instance to array.
        /// </summary>
        internal Command[] ToArray()
        {
            return this.Commands.ToArray();
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            this.Commands.Clear();
            this.Commands = null;
        }
    }
}