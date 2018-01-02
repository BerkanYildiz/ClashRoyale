namespace ClashRoyale.Logic.GameObject
{
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Logic.GameObject.Component;
    using ClashRoyale.Logic.GameObject.Manager;
    using ClashRoyale.Maths;

    public class GameObject
    {
        protected Vector2 Position;
        protected Vector2 PreviousPosition;
        protected List<Component.Component> Components;
        protected GameObjectManager AttachedGameObjectManager;

        protected int PositionZ;
        protected int PreviousPositionZ;

        /// <summary>
        /// Gets if the gameobject is alive.
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                HitpointComponent HitpointComponent = this.HitpointComponent;

                if (HitpointComponent != null)
                {
                    return HitpointComponent.IsAlive;
                }

                return false;
            }
        }
        
        /// <summary>
        /// Gets the hitpoint component.
        /// </summary>
        public HitpointComponent HitpointComponent
        {
            get
            {
                return this.GetComponent<HitpointComponent>(2);
            }
        }

        /// <summary>
        /// Gets the movement component.
        /// </summary>
        public MovementComponent MovementComponent
        {
            get
            {
                return this.GetComponent<MovementComponent>(1);
            }
        }

        /// <summary>
        /// Gets the data of this gameobject.
        /// </summary>
        public CsvData Data
        {
            get;
        }

        /// <summary>
        /// Gets if this gameobject is a summoner.
        /// </summary>
        public virtual bool IsSummoner
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets if this gameobject is a tower.
        /// </summary>
        public virtual bool IsTower
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the type of the gameobject.
        /// </summary>
        public virtual int Type
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        /// <param name="CsvData">The csv data.</param>
        public GameObject(CsvData CsvData)
        {
            this.Data = CsvData;
            this.Position = new Vector2(0x7FFFFFFF, 0x7FFFFFFF);
            this.PreviousPosition = new Vector2();
        }

        /// <summary>
        /// Gets a component by type.
        /// </summary>
        public Component.Component GetComponent(int Type)
        {
            return this.Components.Find(C => C.Type == Type);
        }

        /// <summary>
        /// Gets a component by type.
        /// </summary>
        public T GetComponent<T>(int Type) where T : Component.Component
        {
            return this.Components.Find(C => C.Type == Type) as T;
        }

        /// <summary>
        /// Decodes the specified component.
        /// </summary>
        public void EncodeComponent(ByteStream Stream, int Type)
        {
            if (this.Components.Count > Type)
            {
                this.GetComponent(Type)?.Encode(Stream);
            }
        }

        /// <summary>
        /// Sets the position of gameobject.
        /// </summary>
        public void SetPosition(int X, int Y, int Z)
        {
            if (this.Position.X == 0x7FFFFFFF)
            {
                this.PreviousPositionZ = Z;
                this.PreviousPosition.Set(X, Y);
            }

            this.PositionZ = Z;
            this.Position.Set(X, Y);
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public virtual void Encode(ChecksumEncoder Stream)
        {
            // TODO : Implement GameObject::Encode(ChecksumEncoder).
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public virtual void Tick()
        {
            // TODO : Implement GameObject::Tick().
        }
    }
}