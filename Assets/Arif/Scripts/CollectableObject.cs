using System;
using UnityEngine;

namespace Arif.Scripts
{
    public class CollectableObject : MonoBehaviour
    {

        public CollectableSO collectableProfile;
        [HideInInspector]public bool canCollect;
        public MeshRenderer meshRenderer;

        private void Start()
        {
            canCollect = false;
            meshRenderer.material.color = Color.green;
        }


        public void MakeMeUI()
        {
            meshRenderer.gameObject.layer = 8;
        }

        public void MakeMeNormal()
        {
            meshRenderer.gameObject.layer = 0;
        }
        
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
