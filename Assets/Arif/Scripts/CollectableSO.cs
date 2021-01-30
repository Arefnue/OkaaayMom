using UnityEngine;

namespace Arif.Scripts
{
    [CreateAssetMenu(menuName = "Collectables/Collectable",fileName = "Collectable")]
    public class CollectableSO : ScriptableObject
    {
        public enum CollectableSize
        {
            Big,
            Small
        }
        
        public enum CollectableType
        {
            Book,
            Box,
            Guitar
        }

        public CollectableSize mySize;
        public CollectableType myType;
        public Sprite myUISprite;
        
    }
}
