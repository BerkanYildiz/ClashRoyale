namespace ClashRoyale.Logic.GameObject.Component
{
    using ClashRoyale.Extensions;

    public class HitpointComponent : Component
    {
        public int Size;

        public int Hitpoints;
        public int MaxHitpoints;

        public int LifeTime;
        public int ShieldHitpoints;
        public int ShieldMaxHitpoints;

        /// <summary>
        /// Gets if this gameobject is alive.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return this.Hitpoints > 0;
            }
        }

        /// <summary>
        /// Gets the type of component.
        /// </summary>
        public override int Type
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HitpointComponent"/> class.
        /// </summary>
        public HitpointComponent(GameObject GameObject) : base(GameObject)
        {
            // HitpointComponent.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        /// <param name="Stream"></param>
        public override void Decode(ByteStream Stream)
        {
            // TODO : Implement LogicHitpointComponent::decode().
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public override void Encode(ChecksumEncoder Stream)
        {
            /*
            stream.WriteVInt(this.Size);

            if (this.ShieldMaxHitpoints > 0)
            {
                stream.WriteVInt(this.ShieldHitpoints);
            }
            else if (this.ShieldHitpoints > 0)
            {
                Logging.Error(this.GetType(), "Encode() - Potential offsync.");
            }

            if (this.Hitpoints <= 0)
            {
                stream.WriteVInt(0);
            }

            stream.WriteVInt(this.ShieldMaxHitpoints);

            if (this.ShieldMaxHitpoints > 0)
            {
                stream.WriteVInt(this.ShieldHitpoints);
            }
            */

            // TODO : Implement LogicHitpointComponent::encode().
        }
    }
}