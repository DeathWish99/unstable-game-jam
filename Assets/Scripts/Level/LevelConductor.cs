using System;
using System.Collections.Generic;
using System.Linq;
using Level;
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

    private void Update()
    {
        CheckLevel();
    }

    public void LoadNextLevel()
    {
        Debug.Log("Loading next level");
        activatedEnemyLoaders[_levelCount].SetActive(false);
        _levelCount++;
        InstantiateLoader();
    }

    private void CheckLevel()
    {
        if (activatedEnemyLoaders[_levelCount].GetComponent<EnemyLoader>().isAllEnemiesDestroyed)
        {
            if (_levelCount == enemyLoaders.Count - 1)
            {
                return;
            }
            LoadNextLevel();
        }
    }

    private void InstantiateLoader()
    {
        Debug.Log(_levelCount);
        if (_levelCount < enemyLoaders.Count)
        {
            activatedEnemyLoaders.Add(Instantiate(enemyLoaders[_levelCount]));
        }
    }

    void WinCondition()
    {
        
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
