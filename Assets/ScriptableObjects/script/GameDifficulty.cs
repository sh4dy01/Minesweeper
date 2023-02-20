using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDifficulty", order = 0)]
public class GameDifficulty : ScriptableObject
{
    [SerializeField] public int _width;
    [SerializeField] public int _height;
    [SerializeField] public int _bombQuantity;
    
}
