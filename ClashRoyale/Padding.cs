namespace ClashRoyale
{
    using System;

    public static class Padding
    {
        /// <summary>
        /// Padds the specified message.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="Limit">The limit.</param>
        /// <param name="ReplaceWith">The replace with.</param>
        public static string Pad(this string Message, int Limit = 25, string ReplaceWith = "..")
        {
            if (Message.Length > Limit)
            {
                Message = Message.Substring(0, Limit - ReplaceWith.Length);
                Message = Message + ReplaceWith;
            }
            else if (Message.Length < Limit)
            {
                int Length   = Limit - Message.Length;

                int LeftPad  = (int) Math.Round((double) Length / 2, MidpointRounding.AwayFromZero);
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
