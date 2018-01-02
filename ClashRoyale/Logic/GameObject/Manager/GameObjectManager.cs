namespace ClashRoyale.Logic.GameObject.Manager
{
    using System.Collections.Generic;

    public class GameObjectManager
    {
        public List<GameObject> GameObjects;

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
        public void AddGameObject(GameObject GameObject)
        {
            if (GameObject != null)
            {
                
            }
        }
    }
}