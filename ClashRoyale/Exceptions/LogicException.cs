namespace ClashRoyale.Exceptions
{
    using System;

    public class LogicException : Exception
    {
        /// <summary>
        /// Gets the instance type that threw the exception.
        /// </summary>
        public Type ThrowingClass
        {
            get;
        }

        /// <summary>
        /// Obtient un message qui décrit l'exception actuelle.
        /// </summary>
        public override string Message
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class.
        /// </summary>
        public LogicException()
        {
            // LogicException.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class.
        /// </summary>
        /// <param name="Type">The type.</param>
        public LogicException(Type Type, bool IsLogged = false)
        {
            this.ThrowingClass  = Type;
            this.Message        = "Exception thrown at " + Type.Name + ".";

            if (IsLogged)
            {
                Logging.Error(Type, this.Message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        /// <param name="IsLogged">if set to <c>true</c>, calls the <see cref="Logging"/> class.</param>
        public LogicException(Type Type, string Message, bool IsLogged = true) : base(Message)
        {
            this.ThrowingClass  = Type;
            this.Message        = "(" + Type.Name + ") " + Message;

            if (IsLogged)
            {
                Logging.Error(Type, Message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicException"/> class.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Exception">The exception.</param>
        /// <param name="Message">The message.</param>
        /// <param name="IsLogged">if set to <c>true</c>, calls the <see cref="Logging"/> class.</param>
        public LogicException(Type Type, Exception Exception, string Message = "", bool IsLogged = true) : base(string.IsNullOrEmpty(Message) ? Exception.Message : Message, Exception)
        {
            this.ThrowingClass  = Type;

            if (string.IsNullOrEmpty(Message))
            {
                this.Message    = "(" + Type.Name + ") " + Exception.Message;
            }
            else
            {
                this.Message    = "(" + Type.Name + ") " + Message + Environment.NewLine;
                this.Message   += Exception.Message;
            }

            if (IsLogged)
            {
                Logging.Error(Type, Message);
            }
        }
    }
}
