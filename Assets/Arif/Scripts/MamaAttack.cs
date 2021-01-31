using System;
using UnityEngine;

namespace Arif.Scripts
{
    public class MamaAttack : MonoBehaviour
    {
        public Animator animator;

        
        public void Attack()
        {
            animator.SetTrigger("Spiderman");
        }
        
        
    }
}
