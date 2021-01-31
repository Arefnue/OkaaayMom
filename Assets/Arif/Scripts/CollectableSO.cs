using UnityEngine;

namespace Arif.Scripts
{
    [CreateAssetMenu(menuName = "Collectables/Collectable", fileName = "Collectable")]
    public class CollectableSO : ScriptableObject
    {
        public enum CollectableSize
        {
            Big,
            Small
        }

        public enum CollectableType
        {
            Anahtar,
            Aski,
            Avize,
            Bot,
            Copkutusu,
            Corap,
            Dambil,
            Dart,
            Dolap,
            Evrak,
            Gitar,
            Hali,
            Kanepe,
            KanepeCift,
            Kapi,
            Kazak,
            Kitap,
            KitapSet,
            Kumanda,
            Kumbara,
            Lamba,
            Masa,
            PC,
            Pena,
            Pencere,
            Petek,
            Pijama,
            Raf,
            Resim,
            Saat,
            Saksi,
            Sandalye,
            Sarj,
            Terlik,
            TV,
            Vazo,
            Yatak,
            Yuzuk
        }

        public CollectableSize mySize;
        public CollectableType myType;
        public Sprite myUISprite;
    }
}