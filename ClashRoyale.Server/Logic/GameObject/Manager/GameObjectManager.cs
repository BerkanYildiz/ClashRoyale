namespace ClashRoyale.Server.Logic.GameObject.Manager
{
    using System.Collections.Generic;

    internal class GameObjectManager
    {
        internal List<GameObject> GameObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectManager"/> class.
        /// </summary>
        public GameObjectManager()
        {
            this.GameObjects = new List<GameObject>(64);
        }

        /// <summary>
        /// Adds the specified gameobject.
        /// </summary>
        internal void AddGameObject(GameObject GameObject)
        {
            if (GameObject != null)
            {
                
            }
        }
    }
}