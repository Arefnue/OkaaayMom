using UnityEngine;

namespace Arif.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Manager;

        public Camera mainCam;
        public Camera overlayCam;

        private void Awake()
        {
            Manager = this;
        }
    }
}
