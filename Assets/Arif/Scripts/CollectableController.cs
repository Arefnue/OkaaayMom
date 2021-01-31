using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    public class CollectableController : MonoBehaviour
    {
        public static CollectableController instance;
        public class CollectablePosition
        {
            public CollectableSO.CollectableType MyType = CollectableSO.CollectableType.Book;
            public Vector3 MyPos = Vector3.zero;
            public float Radius = 2f;
        }

        public List<CollectablePosition> CollectablePositionsList = new List<CollectablePosition>();


        private void Awake()
        {
            CollectableObject[] localObjects = FindObjectsOfType<CollectableObject>();

            instance = this;
            
            foreach (var collectableObject in localObjects)
            {
                var clone = new CollectablePosition();
                clone.MyType = collectableObject.collectableProfile.myType;
                clone.MyPos = collectableObject.transform.position;
                CollectablePositionsList.Add(clone);
            }
        }


        public bool ControlDistance(CollectableSO.CollectableType targetType, Vector3 pos)
        {
            foreach (var collectablePosition in CollectablePositionsList)
            {
                if (collectablePosition.MyType == targetType)
                {
                    if ((collectablePosition.MyPos-pos).magnitude<collectablePosition.Radius)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
    }
}
