namespace ClashRoyale.Logic.Commands.Storage
{
    using System;
    using System.Collections.Generic;

    public class CommandStorage : IDisposable
    {
        public List<Command> Commands;

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
        public void AddCommand(Command Command)
        {
            this.Commands.Add(Command);
        }

        /// <summary>
        /// Removes all commands.
        /// </summary>
        public void RemoveCommands()
        {
            this.Commands.Clear();
        }

        /// <summary>
        /// Convertes this instance to array.
        /// </summary>
        public Command[] ToArray()
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