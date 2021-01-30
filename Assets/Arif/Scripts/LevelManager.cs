using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arif.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Manager;

        public enum LevelStates
        {
            Prepare,
            MainGame,
            Finish
        }

        public LevelStates currentLevelState;

        public PlayerController playerController;
        
        [HideInInspector]public List<CollectableObject> collectedObjectsList = new List<CollectableObject>();

        public Transform bagOfHoldingSpawnTransform;

        private void Awake()
        {
            Manager = this;
        }

        private void Start()
        {
            currentLevelState = LevelStates.MainGame;
        }

        private void Update()
        {
            switch (currentLevelState)
            {
                case LevelStates.Prepare:
                    break;
                case LevelStates.MainGame:
                    MovePlayer();
                    SelectObject();
                    break;
                case LevelStates.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void MovePlayer()
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;

                var ray = GameManager.Manager.mainCam.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray,out hit))
                {
                    var ground = hit.collider.GetComponent<Ground>();
                    if (ground)
                    {
                        playerController.playerAgent.SetDestination(hit.point);
                    }
                }
            }
        }
        
        

        public void SelectObject()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                var ray = GameManager.Manager.mainCam.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray,out hit))
                {
                    var collectableObject = hit.collider.GetComponent<CollectableObject>();
                    if (collectableObject)
                    {
                        if (collectableObject.canCollect)
                        {
                           
                            CollectObject(collectableObject);
                            
                        }
                    }
                }
            }
        }
        
        public void CollectObject(CollectableObject collectableObject)
        {
            var cloneObject = Instantiate(collectableObject, bagOfHoldingSpawnTransform);
            cloneObject.transform.position = Vector3.zero;
            cloneObject.transform.rotation = Quaternion.identity;
            cloneObject.transform.localScale = Vector3.one;
                            
            Destroy(collectableObject.gameObject);
        }
        
    }
}
