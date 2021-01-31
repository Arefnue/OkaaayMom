using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Arif.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Manager;

        public enum LevelStates
        {
            Prepare,
            MainGame,
            War,
            Finish
        }

        [Header("Core")]
        [HideInInspector]public LevelStates currentLevelState;
        public PlayerController playerController;
        
        [HideInInspector]public List<CollectableSO> collectedProfileList = new List<CollectableSO>();

        [Header("Bag")]
        public RectTransform bagContentTransform;
        public CollectableImage collectableImagePrefab;
        public int maxItemCount=10;
        
        [Header("UI")]
        public Canvas mainCanvas;

        [Header("Order")]
        public List<CollectableSO> allCollectableProfilesList;
        public int orderCount;
        public Transform orderListTransform;
        [HideInInspector]public List<CollectableSO> orderedCollectableList = new List<CollectableSO>();
        public OrderedImage orderedImagePrefab;
        [HideInInspector]public int rightOrderCount;

        public float maxDayTime=60f;

        public Text timerText;

             
        [HideInInspector]public List<OrderedImage> orderedImageList = new List<OrderedImage>();

        public float credibilityIncreaseValue;
        public float credibilityDecreaseValue;
        public float motherDecreaseValue;

        [HideInInspector] public float credibilityPoint;
        [HideInInspector] public float motherPoint;
        [HideInInspector] public float dayTimer;

        private Vector3 _playerStartPos;

        private void Awake()
        {
            Manager = this;
        }

        private void Start()
        {
           StartGame();
           _playerStartPos = playerController.transform.position;
        }


        public void StartGame()
        {
            currentLevelState = LevelStates.MainGame;
            DetermineOrder();
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
                case LevelStates.War:
                    
                   SelectAtWar();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            dayTimer += Time.deltaTime;

            timerText.text = (maxDayTime - dayTimer).ToString(".0");
            if (dayTimer>= maxDayTime)
            {
                FinishLevel();
                currentLevelState = LevelStates.Finish;
            }

        }


        public void FinishLevel()
        {
            ResetLevel();
            CheckRightness();
        }

        public void ResetLevel()
        {
            dayTimer = 0;
            playerController.transform.position = _playerStartPos;
            
            collectedProfileList?.Clear();
            foreach (var VARIABLE in orderedImageList)
            {
                Destroy(VARIABLE.gameObject);
            }
            orderedImageList?.Clear();
            orderedCollectableList?.Clear();

            foreach (var VARIABLE in _collectableImageList)
            {
                Destroy(VARIABLE.gameObject);
            }
            _collectableImageList?.Clear();
            StartGame();
        }
        
        public void SelectAtWar()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = GameManager.Manager.overlayCam.ScreenPointToRay(Input.mousePosition);
               
                if (Physics.Raycast(ray,out hit))
                {
                   
                    var collectableObject = hit.collider.GetComponent<CollectableObject>();
                    if (collectableObject)
                    {
                        CollectObject(collectableObject);
                    }
                }
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
                            return;
                        }
                    }

                    var interactiveObject = hit.collider.GetComponent<InteractiveObjects>();
                    if (interactiveObject)
                    {
                        if (interactiveObject.canInteract)
                        {
                            interactiveObject.OnInteract();
                        }
                    }
                }
            }
        }

        public void RespawnObject(CollectableObject collectableObject)
        {
            foreach (var so in collectedProfileList)
            {
                if (so.myType == collectableObject.collectableProfile.myType)
                {
                    collectedProfileList.Remove(so);
                    break;
                }
            }
        }
        
        private List<CollectableImage> _collectableImageList = new List<CollectableImage>();
       
        public void CollectObject(CollectableObject collectableObject)
        {

            if (collectedProfileList.Count>=maxItemCount)
            {
                
                Debug.Log("Doldu");
                return;
            }
            var cloneObject = Instantiate(collectableImagePrefab,bagContentTransform);
            _collectableImageList.Add(cloneObject);
            cloneObject.myImage.sprite = collectableObject.collectableProfile.myUISprite;
            collectedProfileList.Add(collectableObject.collectableProfile);
            cloneObject.myObject = collectableObject;
            cloneObject.myObject.gameObject.SetActive(false);
        }
   
        
        public void DetermineOrder()
        {
            for (int i = 0; i < orderCount; i++)
            {
                var randomIndex = Random.Range(0, allCollectableProfilesList.Count);
                orderedCollectableList.Add(allCollectableProfilesList[randomIndex]);
                
                var cloneOrder = Instantiate(orderedImagePrefab,orderListTransform);
                cloneOrder.myImage.sprite = allCollectableProfilesList[randomIndex].myUISprite;
                cloneOrder.myProfile = allCollectableProfilesList[randomIndex];
                
                orderedImageList.Add(cloneOrder);
            }
        }

        public void CheckRightness()
        {
            foreach (var so in collectedProfileList)
            {
                if (orderedCollectableList.Contains(so))
                {
                    orderedCollectableList.Remove(so);
                    rightOrderCount++;
                }
            }

            if (rightOrderCount>=orderedCollectableList.Count)
            {
                credibilityPoint += credibilityIncreaseValue;
            }
            else
            {
                credibilityPoint -= credibilityDecreaseValue;
            }
        }
    }
}
