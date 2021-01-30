using UnityEngine;

namespace Arif.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Manager;

        public Camera mainCam;

        private void Awake()
        {
            Manager = this;
        }
    }
}
