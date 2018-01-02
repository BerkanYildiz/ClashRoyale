namespace ClashRoyale.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;

    public class CharacterBuffComponent : Component
    {
        /// <summary>
        /// Gets the type of component.
        /// </summary>
        public override int Type
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterBuffComponent"/> class.
        /// </summary>
        public CharacterBuffComponent(GameObject GameObject) : base(GameObject)
        {
            // CharacterBuffComponent.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(ByteStream Stream)
        {
            // TODO : Implement LogicCharacterBuffComponent::decode().
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement LogicCharacterBuffComponent::encode().
        }
    }
}