namespace ClashRoyale.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;

    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal static class HandlerFactory
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MessageHandlers"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// The message handler, used to process the received and sent messages.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation token.</param>
        public delegate Task MessageHandler(Device Device, Message Message, CancellationToken Cancellation);

        /// <summary>
        /// The dictionnary of handlers, used to route packet ids and handle them.
        /// </summary>
        public static readonly Dictionary<short, MessageHandler> MessageHandlers = new Dictionary<short, MessageHandler>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (HandlerFactory.Initialized)
            {
                return;
            }

            HandlerFactory.Initialized = true;
        }

        /// <summary>
        /// Handles the specified <see cref="Message"/> using the specified <see cref="Device"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        public static async Task<bool> MessageHandle(Device Device, Message Message)
        {
            using (var Cancellation = new CancellationTokenSource())
            {
                var Token = Cancellation.Token;

                if (HandlerFactory.MessageHandlers.TryGetValue(Message.Type, out MessageHandler Handler))
                {
                    Cancellation.CancelAfter(4000);

                    try
                    {
                        await Handler(Device, Message, Token);
                    }
                    catch (LogicException)
                    {
                        // Handled.
                    }
                    catch (OperationCanceledException)
                    {
                        Logging.Warning(typeof(MessageHandler), "Operation has been cancelled after 4 seconds, while processing " + Message.GetType().Name + ".");
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(typeof(MessageHandler), "Operation has been aborted because of a " + Exception.GetType().Name + ", while processing " + Message.GetType().Name + ".");
                    }

                    if (Cancellation.IsCancellationRequested)
                    {
                        Logging.Warning(typeof(MessageHandler), "Operation has been cancelled after processing " + Message.GetType().Name + ".");
                    }
                }
            }

            return false;
        }
    }
}