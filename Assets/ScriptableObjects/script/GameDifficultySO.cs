using UnityEngine;

namespace ScriptableObjects.script
{
    [CreateAssetMenu(fileName = "GameDifficulty", order = 0)]
    public class GameDifficultySo : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _bombQuantity;

        public int Width => _width;
        public int Height => _height;
        public int BombQuantity => _bombQuantity;
    }
}
