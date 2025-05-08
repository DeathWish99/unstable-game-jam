using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelConductor : MonoBehaviour
{
    public List<GameObject> enemyLoaders;

    private List<GameObject> activatedEnemyLoaders;
    private Camera _mainCamera;
    private int _levelCount = 0;
    void Start()
    {
        activatedEnemyLoaders = new List<GameObject>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        enemyLoaders[_levelCount].transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, 0f);
        InstantiateLoader();
    }

    public void LoadNextLevel()
    {
        activatedEnemyLoaders[_levelCount].SetActive(false);
        _levelCount++;
        InstantiateLoader();
    }

    private void InstantiateLoader()
    {
        Instantiate(enemyLoaders[_levelCount]);
        activatedEnemyLoaders.Add(enemyLoaders[_levelCount]);
    }
    
    // private void MoveEnemiesToEdgeOfMap()
    // {
    //     var enemies = enemyLoaders[_levelCount].GetComponentsInChildren<Transform>();
    //
    //     foreach (var enemy in enemies)
    //     {
    //         
    //     }
    // }
}
