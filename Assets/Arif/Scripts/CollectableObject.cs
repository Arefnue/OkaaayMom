using System;
using UnityEngine;

namespace Arif.Scripts
{
    public class CollectableObject : MonoBehaviour
    {

        public CollectableSO collectableProfile;
        [HideInInspector]public bool canCollect;
        public MeshRenderer meshRenderer;
        public Rigidbody rb;

        private Color _initalColor=new Color();
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            canCollect = false;
            if (meshRenderer)
            {
                _initalColor = meshRenderer.material.color;
            }
           

        }


        public void MakeMeUI()
        {
          
            var children = gameObject.GetComponentsInChildren<Transform>();
            foreach (var VARIABLE in children)
            {
                VARIABLE.gameObject.layer = 8;
            }
        }

        public void MakeMeNormal()
        {
            var children = gameObject.GetComponentsInChildren<Transform>();
            foreach (var VARIABLE in children)
            {
                VARIABLE.gameObject.layer = 0;
            }
           
        }
        
        public void OnPlayerEnter()
        {
            canCollect = true;
            if (meshRenderer)
            {
                meshRenderer.material.color = Color.cyan;
                
            }
            
        }

        public void OnPlayerExit()
        {
            canCollect = false;
            if (meshRenderer)
            {
                meshRenderer.material.color = _initalColor;
                
            }
            
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
