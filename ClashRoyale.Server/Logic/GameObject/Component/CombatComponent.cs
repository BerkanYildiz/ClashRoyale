namespace ClashRoyale.Server.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;

    internal class CombatComponent : Component
    {
        /// <summary>
        /// Gets the type of component.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatComponent"/> class.
        /// </summary>
        public CombatComponent(GameObject GameObject) : base(GameObject)
        {
            // CombatComponent.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        /// <param name="Stream"></param>
        internal override void Decode(ByteStream Stream)
        {
            // TODO : Implement LogicCombatComponent::decode().
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal override void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement LogicCombatComponent::encode().
        }
    }
}