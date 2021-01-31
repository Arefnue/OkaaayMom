using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arif.Scripts
{
    public class CollectableImage : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        public Image myImage;
        [HideInInspector]public CollectableObject myObject;

        private Transform _myParent;

        private Vector2 _lastPos;
        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastPos = transform.position;
            _myParent = transform.parent;
            transform.SetParent(LevelManager.Manager.mainCanvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (LevelManager.Manager.currentLevelState == LevelManager.LevelStates.War)
            {
                RaycastHit hit;
                var ray = GameManager.Manager.overlayCam.ScreenPointToRay(Input.mousePosition);
               
                if (Physics.Raycast(ray,out hit))
                {
                    var cloneObject = Instantiate(myObject,hit.collider.GetComponent<Transform>().parent);
                    cloneObject.transform.position = hit.point;
                    cloneObject.gameObject.SetActive(true);
                    cloneObject.MakeMeUI();
                    LevelManager.Manager.RespawnObject(cloneObject);
                    
                    if (!CollectableController.instance.ControlDistance(cloneObject.collectableProfile.myType,cloneObject.transform.position))
                    {
                        LevelManager.Manager.motherPoint++;
                    }
                    
                    Destroy(gameObject);
                }
                else
                {
                    transform.SetParent(_myParent);
                    transform.position = _lastPos;
                
                }
            }
            else
            {
                RaycastHit hit;
                var ray = GameManager.Manager.mainCam.ScreenPointToRay(Input.mousePosition);
               
                if (Physics.Raycast(ray,out hit))
                {
                    var cloneObject = Instantiate(myObject,LevelManager.Manager.transform);
                    cloneObject.transform.position = hit.point;
                    cloneObject.gameObject.SetActive(true);
                    cloneObject.MakeMeNormal();
                    LevelManager.Manager.RespawnObject(cloneObject);
                    
                    if (!CollectableController.instance.ControlDistance(cloneObject.collectableProfile.myType,cloneObject.transform.position))
                    {
                        LevelManager.Manager.motherPoint++;
                    }
                    Destroy(gameObject);
                }
                else
                {
                    transform.SetParent(_myParent);
                    transform.position = _lastPos;
                
                }
            }
            
        }
    }
}
