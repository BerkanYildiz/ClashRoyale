namespace ClashRoyale.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;

    public class MovementComponent : Component
    {
        /// <summary>
        /// Gets the type of component.
        /// </summary>
        public override int Type
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementComponent"/> class.
        /// </summary>
        public MovementComponent(GameObject GameObject) : base(GameObject)
        {
            // MovementComponent.
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Decode(ByteStream Stream)
        {
            // TODO : Implement LogicMovementComponent::decode().
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement LogicMovementComponent::encode().
        }
    }
}