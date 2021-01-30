using UnityEngine;

namespace Arif.Scripts
{
    public class CollectableObject : MonoBehaviour
    {
        public Transform movePos;
        public bool useMovePos;
        
        public void OnClicked()
        {
            if (useMovePos)
            {
                LevelManager.Manager.playerController.playerAgent.SetDestination(movePos.position);
            }
            else
            {
                LevelManager.Manager.playerController.playerAgent.SetDestination(transform.position);
            }
            
        }
        
        public void OnCollected()
        {
            
        }


        public void OnReleased()
        {
            
        }

        public void OnUsed()
        {
            
        }
        
        
    }
}
