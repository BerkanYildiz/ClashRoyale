namespace ClashRoyale.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.GameObject.Manager;

    public class Component
    {
        /// <summary>
        /// Gets the battle.
        /// </summary>
        public Battle Battle
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the gameobject manager.
        /// </summary>
        public GameObjectManager GameObjectManager
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parent of component.
        /// </summary>
        public GameObject Parent
        {
            get;
        }

        /// <summary>
        /// Gets the type of component.
        /// </summary>
        public virtual int Type
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
        public virtual void Decode(ByteStream Stream)
        {
            // TODO : Implement Component::Decode(ByteStream).
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public virtual void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement Component::Encode(ChecksumEncoder).
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public virtual void Tick()
        {
            // TODO : Implement Component::Tick().
        }
    }
}