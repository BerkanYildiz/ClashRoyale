namespace ClashRoyale.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;

    public class CombatComponent : Component
    {
        /// <summary>
        /// Gets the type of component.
        /// </summary>
        public override int Type
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
        public override void Decode(ByteStream Stream)
        {
            // TODO : Implement LogicCombatComponent::decode().
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement LogicCombatComponent::encode().
        }
    }
}