using System;
using UnityEngine;

namespace Arif.Scripts
{
    public class InteractiveObjects : MonoBehaviour
    {
        public GameObject myPanel;
        public MeshRenderer meshRenderer;
        [HideInInspector]public bool canInteract;

        public void OnInteract()
        {
            LevelManager.Manager.currentLevelState = LevelManager.LevelStates.War;
            myPanel.SetActive(true);
            
        }

        public void DisablePanel()
        {
            LevelManager.Manager.currentLevelState = LevelManager.LevelStates.MainGame;
            myPanel.SetActive(false);
        }
        
        public void OnPlayerEnter()
        {
            canInteract = true;
            meshRenderer.material.color = Color.white;
        }

        public void OnPlayerExit()
        {
            canInteract = false;
            meshRenderer.material.color = Color.blue;
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
