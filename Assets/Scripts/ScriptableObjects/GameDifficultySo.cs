using UnityEngine;

namespace ScriptableObjects.script
{
    [CreateAssetMenu(fileName = "GameDifficulty", order = 0)]
    public class GameDifficultySo : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _bombQuantity;
        [SerializeField] private int _countdown;

        public int Width => _width;
        public int Height => _height;
        public int BombQuantity => _bombQuantity;
        
        public void SetWidth(int width)
        {
            _width = width;
        }
        
        public void SetHeight(int height)
        {
            _height = height;
        }
        
        public void SetBombQuantity(int bombQuantity)
        {
            _bombQuantity = bombQuantity;
        }
        public int Countdown => _countdown;
    }
}