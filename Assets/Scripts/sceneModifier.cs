using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  

public class sceneModifier : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("game");
    }

    public void LobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
