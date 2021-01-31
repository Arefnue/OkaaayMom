using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [Header("UIManager")] public Texture2D[] MomImgs;
        public Image MomImage;
        
        public float credibilityPoint
        {
            get
            {
                return _credibilityPoint;
            }
            set
            {
                _credibilityPoint = value;
                _credibilityPoint = Mathf.Clamp(_credibilityPoint, 0, 1f);
                crediSlider.value = _credibilityPoint;
                //todo bitir
                if (_credibilityPoint>=1f)
                {
                    playerController.playerAnimator.SetTrigger("Spiderman");
                    currentLevelState = LevelStates.Finish;
                    
                    Win();
                }
                else if (_credibilityPoint<=0f)
                {
                    playerController.playerAnimator.SetTrigger("Spiderman");
                    currentLevelState = LevelStates.Finish;
                    Lose();
                }
            }
        }

        private void Lose()
        {
            StartCoroutine(nameof(DelayLose));
        }

        private IEnumerator DelayLose()
        {
            yield return new WaitForSeconds(3f);
            losePanel.SetActive(true);
        }

        private void Win()
        {
            StartCoroutine(DelayWin());
        }

        private IEnumerator DelayWin()
        {
            yield return new WaitForSeconds(3f);
            winPanel.SetActive(true);
        }
        
        private float _credibilityPoint = 0.5f;

        public float motherPoint
        {
            get
            {
                return _motherPoint;
            }
            set
            {
                _motherPoint = value;
                _motherPoint = Mathf.Clamp(_motherPoint, 0, 1f);
                mamaSlider.value = _motherPoint;
                
                //todo UI ekle
                if (_motherPoint>=1f)
                {
                    MamaAttack();
                    currentLevelState = LevelStates.Finish;
                    Lose();
                }

            }
        }

        public void Retry()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        private float _motherPoint = 0f;

        [HideInInspector]public float dayTimer;

        public Transform cameraRoot;
        public Transform mainCamPos;

        private Vector3 _playerStartPos;

        public List<MamaAttack> mamaList;

        public Slider mamaSlider;
        public Slider crediSlider;

        public GameObject winPanel;
        public GameObject losePanel;
        public GameObject dayPanel;

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
            playerController.playerAnimator.SetBool("Run",true);
        }

        public void MamaAttack()
        {
            foreach (var VARIABLE in mamaList)
            {
                VARIABLE.gameObject.SetActive(true);
                VARIABLE.Attack();
            }
        }
        
        private void Update()
        {
            switch (currentLevelState)
            {
                case LevelStates.Prepare:
                    break;
                case LevelStates.MainGame:
                    MovePlayer();
                    //MoveWithKeyboard();
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


            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController.playerAnimator.SetTrigger("Spiderman");
            }
            
            if (currentLevelState != LevelStates.Finish && currentLevelState != LevelStates.Prepare)
            {
                dayTimer += Time.deltaTime;

                timerText.text = (maxDayTime - dayTimer).ToString(".0");
                if (dayTimer>= maxDayTime)
                {
                    currentLevelState = LevelStates.Finish;
                    ShowDayPanel();
                
                }

                
               
            }
            

        }
        

        private void LateUpdate()
        {
            switch (currentLevelState)
            {
                case LevelStates.Prepare:
                    break;
                case LevelStates.MainGame:
                    cameraRoot.transform.position = playerController.transform.position;
                    break;
                case LevelStates.War:
                    break;
                case LevelStates.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SkipDay()
        {
            FinishLevel();
        }

        public void ShowDayPanel()
        {
            dayPanel.SetActive(true);
        }
        
        
        public void FinishLevel()
        {
            
            CheckRightness();
            ResetLevel();
        }

        public void ResetLevel()
        {
            dayTimer = 0;
            playerController.transform.position = _playerStartPos;
            playerController.playerAgent.SetDestination(_playerStartPos);
            var orderedChildren = orderListTransform.GetComponentsInChildren<OrderedImage>();
            
            foreach (var VARIABLE in orderedChildren)
            {
                Destroy(VARIABLE.gameObject);
            }
            orderedImageList?.Clear();
            orderedCollectableList?.Clear();
            
            dayPanel.SetActive(false);
            losePanel.SetActive(false);
            winPanel.SetActive(false);
            
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

        public void MoveWithKeyboard()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            
            var moveDir = new Vector3(horizontal,0,vertical);

            playerController.transform.position += moveDir * (5 * Time.deltaTime);
                
            playerController.transform.localRotation = Quaternion.LookRotation(moveDir);

        

        }

        /*private void FixedUpdate()
        {
            Vector3 movedir = Vector3.zero;
            movedir.x = Input.GetAxis("Horizontal");
            movedir.z = Input.GetAxis("Vertical");
            playerController.myRb.velocity = movedir*5;
            playerController.myRb.angularVelocity =new Vector3(0,playerController.myRb.angularVelocity.y,0); 


        }*/

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

            collectableObject.rb.isKinematic = false;
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
