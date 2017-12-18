namespace ClashRoyale
{
    using System;
    using System.Diagnostics;

    public static class Logging
    {
        /// <summary>
        /// Debuggings the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        [Conditional("DEBUG")]
        public static void Debugging(Type Type, string Message)
        {
            // Resources.Logger.Info(Type.Name + " : " + Message);
            Debug.WriteLine("[ DEBUG ] " + Logging.Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        [Conditional("DEBUG")]
        public static void Info(Type Type, string Message)
        {
            // Resources.Logger.Info(Type.Name + " : " + Message);
            Debug.WriteLine("[ INFO  ] " + Logging.Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Warning(Type Type, string Message)
        {
            // Resources.Logger.Warn(Type.Name + " : " + Message);
            Debug.WriteLine("[WARNING] " + Logging.Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Error(Type Type, string Message)
        {
            // Resources.Logger.Error(Type.Name + " : " + Message);
            Debug.WriteLine("[ ERROR ] " + Logging.Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        public static void Fatal(Type Type, string Message)
        {
            // Resources.Logger.Fatal(Type.Name + " : " + Message);
            Debug.WriteLine("[ FATAL ] " + Logging.Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Padds the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="Limit">The limit.</param>
        /// <param name="ReplaceWith">The replace with.</param>
        private static string Padding(string Message, int Limit = 25, string ReplaceWith = "..")
        {
            if (Message.Length > Limit)
            {
                Message = Message.Substring(0, Limit - ReplaceWith.Length);
                Message = Message + ReplaceWith;
            }
            else if (Message.Length < Limit)
            {
                int Length = Limit - Message.Length;

                int LeftPad = (int) Math.Round((double) Length / 2, MidpointRounding.AwayFromZero);
                int RightPad = (int) Math.Round((double) Length / 2, MidpointRounding.AwayFromZero);

                if (Length % 2 != 0)
                {
                    RightPad = RightPad - 1;
                }

                for (int i = 0; i < RightPad; i++)
                {
                    Message += " ";
                }

                for (int i = 0; i < LeftPad; i++)
                {
                    Message = " " + Message;
                }
            }

            return Message;
        }
    }
}