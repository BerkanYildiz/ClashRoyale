namespace ClashRoyale.Client.Packets
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Client.Packets.Messages.Client;
    using ClashRoyale.Client.Packets.Messages.Server;

    internal class MessageFactory
    {
        internal static Dictionary<ushort, Type> Messages = new Dictionary<ushort, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFactory"/> class.
        /// </summary>
        internal MessageFactory()
        {
            MessageFactory.Messages.Add(10100, typeof(Pre_Authentification));
            MessageFactory.Messages.Add(10101, typeof(Authentification));

            // MessageFactory.Messages.Add(10107, typeof(Client_Capabilities));
            MessageFactory.Messages.Add(19911, typeof(Keep_Alive));
            // MessageFactory.Messages.Add(10113, typeof(Get_Device_Token));

            // MessageFactory.Messages.Add(14102, typeof(Execute_Commands));
            // MessageFactory.Messages.Add(14109, typeof(Go_Home));

            // --------------------------------------------------------------------- //

            MessageFactory.Messages.Add(20100, typeof(Pre_Authentification_OK));
            MessageFactory.Messages.Add(20103, typeof(Authentification_Failed));
            MessageFactory.Messages.Add(22280, typeof(Authentification_OK));
            MessageFactory.Messages.Add(24135, typeof(Keep_Alive_OK));

            /* MessageFactory.Messages.Add(20161, typeof(Server_Shutdown));

            MessageFactory.Messages.Add(21903, typeof(Sector_State));

            MessageFactory.Messages.Add(24101, typeof(Own_Home_Data));
            MessageFactory.Messages.Add(24104, typeof(Out_Of_Sync));

            MessageFactory.Messages.Add(24115, typeof(Server_Error));

            MessageFactory.Messages.Add(25892, typeof(Disconnected)); */
        }
    }
}