using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GameObject block;
    public GameObject[,] grid = new GameObject[10, 10];
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 10; i++)
        {
            SpawnGrid();
        }
    }

    void SpawnGrid()
    {
        int x = Random.Range(0, 10);
        int y = Random.Range(0, 10);

        if (grid[x, y] == null)
        {
            Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
            block.GetComponent<Block>().SetBomb();
            Debug.Log("bomb x: " + gameObject.transform.position.x + " y: " + gameObject.transform.position.y);
        } else SpawnGrid();
    }
}
