using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PlayerBodyModel
    {
        public PlayerBodyModel(GameObject startingBody, GameObject playerTail, GameObject objectToSpawn, int maxPlayerSize)
        {
            PlayerSegments = new List<GameObject>()
            {
                startingBody
            };

            TailObject = playerTail;
            ObjectToSpawn = objectToSpawn;
            MaxPlayerSize = maxPlayerSize;
        }

        public List<GameObject> PlayerSegments { get; private set; }
        public int SegmentCount { get; private set; }
        public int SegmentCountActual => PlayerSegments.Count;
        private int MaxPlayerSize { get; set; }
        private GameObject TailObject { get; set; }
        private GameObject ObjectToSpawn { get; set; }

        /// <summary>
        /// Increases the player's visible body length if possible and safe to do so.
        /// </summary>
        /// <returns>A <see cref="bool"/> indicating whether or not the player can actually increase in size anymore and therefore if this method should continue to be called.</returns>
        public bool IncreasePlayerSize()
        {
            if (SegmentCountActual < MaxPlayerSize)
            {
                var lastSegment = PlayerSegments.LastOrDefault();

                if (lastSegment == null)
                {
                    throw new UnassignedReferenceException("PlayerSegments contains no objects, unable to increase player size.");
                }

                var spawnPos = lastSegment.transform.position - new Vector3(0, lastSegment.transform.localScale.y, 0);
                var newPart = GameObject.Instantiate(ObjectToSpawn, spawnPos, new Quaternion());
                var joint = newPart.GetComponent<RelativeJoint2D>();

                joint.connectedBody = lastSegment.GetComponent<Rigidbody2D>();
                joint.autoConfigureOffset = false;
                PlayerSegments.Add(newPart);

                var tailJoint = TailObject.GetComponent<RelativeJoint2D>();

                TailObject.transform.Translate(new Vector3(0, -newPart.transform.localScale.y, 0));
                tailJoint.connectedBody = newPart.GetComponent<Rigidbody2D>();
                tailJoint.autoConfigureOffset = false;

                UpdateSegmentCount();

                if (SegmentCountActual < MaxPlayerSize)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the player's body length for score or record purposes without instantiating new objects.
        /// </summary>
        public void UpdateSegmentCount()
        {
            SegmentCount += 1;
        }
    }
}
