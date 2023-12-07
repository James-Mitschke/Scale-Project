using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class ObjectToSpawn
    {
        public ObjectToSpawn(GameObject gameObject, Vector2 spawnPosition, Quaternion spawnRotation = new(), float sizeModifier = 1)
        {
            Object = gameObject;
            SpawnPosition = spawnPosition;
            SpawnRotation = spawnRotation;
            SizeModifier = sizeModifier;
        }

        /// <summary>
        /// Gets or sets the gameobject
        /// </summary>
        public GameObject Object { get; set; }

        /// <summary>
        /// Gets or sets the obstacle's spawn position
        /// </summary>
        public Vector2 SpawnPosition { get; set; }

        /// <summary>
        /// Gets or sets the obstacle's spawn rotation
        /// </summary>
        public Quaternion SpawnRotation { get; set; } = new();

        /// <summary>
        /// Gets or sets the obstacle's size modifier
        /// </summary>
        public float SizeModifier { get; set; } = 1;
    }
}
