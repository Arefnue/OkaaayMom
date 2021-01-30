using UnityEngine;

namespace Arif.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Manager;


        private void Awake()
        {
            Manager = this;
        }
    }
}
