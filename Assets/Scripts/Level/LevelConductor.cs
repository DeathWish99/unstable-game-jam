using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

public class LevelConductor : MonoBehaviour
{
    public List<GameObject> enemyLoaders;

    public float waveSpawnDelay = 0f;

    private List<GameObject> activatedEnemyLoaders;
    private Camera _mainCamera;
    private int _levelCount = 0;
    private float _waveSpawnDelay;
    void Start()
    {
        _waveSpawnDelay = waveSpawnDelay;
        activatedEnemyLoaders = new List<GameObject>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        enemyLoaders[_levelCount].transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, 0f);
        InstantiateLoader();
        SetLoaderActive();
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
        _waveSpawnDelay = waveSpawnDelay;
        SetLoaderActive();
    }

    private void CheckLevel()
    {
        if (activatedEnemyLoaders[_levelCount].GetComponent<EnemyLoader>().isAllEnemiesDestroyed)
        {
            if (_levelCount == enemyLoaders.Count - 1)
            {
                return;
            }
            _waveSpawnDelay -= Time.deltaTime;
            
            if(_waveSpawnDelay <= 0)
                LoadNextLevel();
        }
    }

    private void InstantiateLoader()
    {
        for (int i = 0; i < enemyLoaders.Count; i++)
        {
            activatedEnemyLoaders.Add(Instantiate(enemyLoaders[i]));
            activatedEnemyLoaders[i].SetActive(false);
        }
        Debug.Log(activatedEnemyLoaders.Count());
    }

    private void SetLoaderActive()
    {
        
        if (_levelCount < enemyLoaders.Count)
        {
            activatedEnemyLoaders[_levelCount].SetActive(true);
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
