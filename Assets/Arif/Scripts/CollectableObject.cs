using System;
using UnityEngine;

namespace Arif.Scripts
{
    public class CollectableObject : MonoBehaviour
    {

        public Sprite mySprite;
        [HideInInspector]public bool canCollect;
        public MeshRenderer meshRenderer;
        
        public void OnPlayerEnter()
        {
            canCollect = true;
            meshRenderer.material.color = Color.white;
        }

        public void OnPlayerExit()
        {
            canCollect = false;
            meshRenderer.material.color = Color.green;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player)
            {
                OnPlayerEnter();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player)
            {
                OnPlayerExit();
            }
        }
        
        
    }
}
