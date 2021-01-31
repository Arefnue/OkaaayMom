using UnityEngine;
using UnityEngine.AI;

namespace Arif.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public Transform modelRoot;
        public NavMeshAgent playerAgent;
        public Collider hardCollider;
        public Collider triggerCollider;
        public Rigidbody myRb;
        public Animator playerAnimator;
    }
}
