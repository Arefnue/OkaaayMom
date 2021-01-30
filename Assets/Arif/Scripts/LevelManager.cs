using System;
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

        private void Awake()
        {
            Manager = this;
        }

        private void Update()
        {
            switch (currentLevelState)
            {
                case LevelStates.Prepare:
                    break;
                case LevelStates.MainGame:
                    break;
                case LevelStates.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
