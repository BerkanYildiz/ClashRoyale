﻿namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;

    internal class AvatarStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 29567;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public AvatarStreamMessage(Device Device) : base(Device)
        {
            // AvatarStreamMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            // this.Stream.WriteVInt(0);
            this.Stream.AddRange("1C-0B-00-00-00-02-38-C4-7D-38-01-00-00-00-02-00-00-37-EE-00-00-00-06-42-65-72-6B-61-6E-00-A3-EB-E4-02-00-00-01-04-59-E8-F1-00-00-00-85-7B-22-61-22-3A-22-32-76-32-20-54-6F-75-63-68-64-6F-77-6E-20-44-61-69-6C-79-20-50-72-61-63-74-69-63-65-22-2C-22-62-22-3A-22-69-63-6F-6E-5F-74-6F-75-72-6E-61-6D-65-6E-74-5F-74-6F-75-63-68-64-6F-77-6E-22-2C-22-64-22-3A-22-74-6F-75-72-6E-61-6D-65-6E-74-5F-6F-70-65-6E-5F-77-69-6E-73-5F-62-61-64-67-65-5F-62-72-6F-6E-7A-65-22-2C-22-65-22-3A-31-31-2C-22-66-22-3A-74-72-75-65-2C-22-67-22-3A-74-72-75-65-7D-07-00-00-00-02-3B-E3-89-67-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-8E-81-BF-02-00-01-1C-0D-07-00-00-00-02-46-CD-1C-3B-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-9B-A8-BE-01-00-01-1C-0D-07-00-00-00-02-48-60-33-F3-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A5-E3-AA-01-00-01-1A-1F-07-00-00-00-02-48-60-34-18-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A5-E3-AA-01-00-01-1A-1F-07-00-00-00-02-48-60-34-36-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A5-E3-AA-01-00-01-1A-1F-07-00-00-00-02-48-60-34-60-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A5-E3-AA-01-00-01-1A-1F-07-00-00-00-02-48-60-35-2F-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A4-E3-AA-01-00-01-1A-1F-07-00-00-00-02-48-60-35-70-01-00-00-00-2D-00-34-0B-0C-00-00-00-07-53-6F-62-72-69-6E-6F-0A-A4-E3-AA-01-00-01-1A-1F-07-00-00-00-02-4B-54-EF-9C-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-85-EE-89-01-00-01-1A-1F-07-00-00-00-02-4B-54-EF-CE-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-85-EE-89-01-00-01-1A-1F-07-00-00-00-02-4B-54-EF-E6-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-85-EE-89-01-00-01-1A-1F-07-00-00-00-02-4B-54-EF-FD-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-85-EE-89-01-00-01-1A-1F-07-00-00-00-02-4B-54-F0-2C-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-84-EE-89-01-00-01-1A-1F-07-00-00-00-02-4B-54-F0-3B-01-00-00-00-1D-00-06-28-0A-00-00-00-07-45-64-67-61-72-63-76-0A-84-EE-89-01-00-01-1A-1F-07-00-00-00-02-4F-F0-23-B7-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-4F-F0-23-C9-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-4F-F0-23-CB-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-4F-F0-23-CE-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-4F-F0-23-E0-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-4F-F0-23-FF-01-00-00-00-2C-00-A5-D5-7E-00-00-00-0C-41-4A-41-20-43-45-4D-41-52-4E-41-54-09-AF-C5-54-00-01-1A-1F-07-00-00-00-02-50-29-35-97-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-99-E4-51-00-01-1A-1F-07-00-00-00-02-50-29-35-D8-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-99-E4-51-00-01-1A-1F-07-00-00-00-02-50-29-35-F2-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-99-E4-51-00-01-1A-1F-07-00-00-00-02-50-29-36-0C-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-98-E4-51-00-01-1A-1F-07-00-00-00-02-50-29-36-21-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-98-E4-51-00-01-1A-1F-07-00-00-00-02-50-29-36-45-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-98-E4-51-00-01-1A-1F-06-00-00-00-02-54-01-D2-55-01-00-00-00-03-00-8C-D1-45-00-00-00-02-3A-56-09-A1-A8-28-00-00-00-00-13-68-61-67-61-6E-20-63-6F-72-6F-6E-61-73-20-70-75-74-6F-73-01-00-00-00-03-00-8C-D1-45-00-00-00-07-63-6F-72-6F-6E-61-73-00-00-00-1E-00-0D-23-57-00-00-00-0B-6C-6F-73-20-70-69-74-75-64-6F-73-10-B2-01".HexaToBytes());
        }
    }
}