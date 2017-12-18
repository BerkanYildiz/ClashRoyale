namespace ClashRoyale.Server.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic.Battle;
    using ClashRoyale.Server.Logic.GameObject.Manager;

    internal class Component
    {
        /// <summary>
        /// Gets the battle.
        /// </summary>
        internal Battle Battle
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the gameobject manager.
        /// </summary>
        internal GameObjectManager GameObjectManager
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parent of component.
        /// </summary>
        internal GameObject Parent
        {
            get;
        }

        /// <summary>
        /// Gets the type of component.
        /// </summary>
        internal virtual int Type
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        public Component(GameObject Parent)
        {
            this.Parent = Parent;
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Decode(ByteStream Stream)
        {
            // TODO : Implement Component::Decode(ByteStream).
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement Component::Encode(ChecksumEncoder).
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal virtual void Tick()
        {
            // TODO : Implement Component::Tick().
        }
    }
}