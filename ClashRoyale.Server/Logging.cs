namespace ClashRoyale.Server
{
    using System;
    using System.Diagnostics;

    internal static class Logging
    {
        /// <summary>
        /// Debuggings the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        [Conditional("DEBUG")]
        internal static void Debugging(Type Type, string Message)
        {
            // Resources.Logger.Info(Type.Name + " : " + Message);
            Debug.WriteLine("[ DEBUG ] " + Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        [Conditional("DEBUG")]
        internal static void Info(Type Type, string Message)
        {
            // Resources.Logger.Info(Type.Name + " : " + Message);
            Debug.WriteLine("[ INFO  ] " + Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        internal static void Warning(Type Type, string Message)
        {
            // Resources.Logger.Warn(Type.Name + " : " + Message);
            Debug.WriteLine("[WARNING] " + Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        internal static void Error(Type Type, string Message)
        {
            // Resources.Logger.Error(Type.Name + " : " + Message);
            Debug.WriteLine("[ ERROR ] " + Padding(Type.Name, 16) + " : " + Message);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <param name="Message">The message.</param>
        internal static void Fatal(Type Type, string Message)
        {
            // Resources.Logger.Fatal(Type.Name + " : " + Message);
            Debug.WriteLine("[ FATAL ] " + Padding(Type.Name, 16) + " : " + Message);
        }

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