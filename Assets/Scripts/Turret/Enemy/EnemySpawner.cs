using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;

    public static EnemySpawner instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemySpawner>();
            }

            return _instance;
        }
    }
    
    [SerializeField] private List<GameObject> enemyPrefabs;
    private List<GameObject> spawnedEnemies;
    [SerializeField] private int poolAmount = 10;
    private List<List<TurretMechanic>> spawnPools;
    private List<int> poolIndexes;
    private List<TurretMechanic> spawnedTurrets;
    [SerializeField] private GameObject spawnerParent;
    [SerializeField] private float timeBetweenSpawns = 3f;
    private List<GameObject> spawnPoints;
    private bool increaseEnabled = true;

    private IEnumerator spawnCor;
    

    private void Awake()
    {
        GameOver.instance.GameOverActive += StopSpawning;
        
        spawnPoints = new List<GameObject>();
        foreach (Transform child in spawnerParent.transform)
        {
            spawnPoints.Add(child.gameObject);
        }

        spawnedEnemies = new List<GameObject>();
        spawnedTurrets = new List<TurretMechanic>();
        CreateSpawnPool();
    }

    public void CreateSpawnPool()
    {
        spawnPools = new List<List<TurretMechanic>>();
        poolIndexes = new List<int>();
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            poolIndexes.Add(0);
            List<TurretMechanic> newPool = new List<TurretMechanic>();
            for (int j = 0; j < poolAmount; j++)
            {
                var newEnemy = SpawnEnemy(i, 0);
                newEnemy.transform.parent.gameObject.SetActive(false);
                newPool.Add(newEnemy);
            }
            spawnPools.Add(newPool);
        }
    }

    public TurretMechanic GetNextFromPool(int poolIndex)
    {
        var retrievedTurret = spawnPools[poolIndex][poolIndexes[poolIndex]];
        poolIndexes[poolIndex] = poolIndexes[poolIndex] + 1 >= spawnPools[poolIndex].Count ? 0 :poolIndexes[poolIndex]+1;
        return retrievedTurret;
    }

    public void StartSpawning()
    {
        increaseEnabled = true;
        if(spawnCor!=null)StopCoroutine(spawnCor);
        spawnCor = SpawnEnemies();
        StartCoroutine(spawnCor);
    }

    public void StopSpawning()
    {
        if(spawnCor!=null)StopCoroutine(spawnCor);
        spawnCor = null;
    }

    IEnumerator SpawnEnemies()
    {
        List<int> alreadyUsedSpawn = new List<int>();
        while(true)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            if(alreadyUsedSpawn.Count==spawnPoints.Count)alreadyUsedSpawn = new List<int>();
            int randomSpawnIndex = Random.Range(0, spawnPoints.Count);
            while (alreadyUsedSpawn.Contains(randomSpawnIndex))
            {
                randomSpawnIndex = Random.Range(0, spawnPoints.Count);
            }
            alreadyUsedSpawn.Add(randomSpawnIndex);
            
            var newTurret=GetNextFromPool(randomIndex);
            var newEnemy = newTurret.transform.parent.gameObject;
            newEnemy.SetActive(true);
            
            newEnemy.transform.position = spawnPoints[randomSpawnIndex].transform.position;
            EnableAIMovement(newEnemy,true);
            spawnedEnemies.Add(newEnemy);
            spawnedTurrets.Add(newTurret);
            
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public TurretMechanic SpawnEnemy(int enemyIndex,int spawnIndex)
    {
        var newEnemy=Instantiate(enemyPrefabs[enemyIndex], transform);
        var turretComp = newEnemy.GetComponentInChildren<TurretMechanic>();
        turretComp.TurretDestroy += OnTurretDestroyed;
        AIDestinationSetter aiDestinationSetter = newEnemy.GetComponent<AIDestinationSetter>();
        aiDestinationSetter.target = PlayerController.instance.turretMechanic.transform;
        EnableAIMovement(newEnemy,false);
        return turretComp;
    }

    public void OnTurretDestroyed(TurretMechanic turretMechanic)
    {
        HideTurret(turretMechanic);
        if(increaseEnabled)Score.instance.IncreaseScore();
    }

    public void HideTurret(TurretMechanic turretMechanic)
    {
        turretMechanic.RestoreLifeBar();
        int turretIndex = spawnedTurrets.IndexOf(turretMechanic);
        spawnedEnemies[turretIndex].transform.localScale=Vector3.one;
        spawnedEnemies[turretIndex].SetActive(false);
        EnableAIMovement(spawnedEnemies[turretIndex],false);
        spawnedEnemies.RemoveAt(turretIndex);
        spawnedTurrets.RemoveAt(turretIndex);
    }

    public void EnableAIMovement(GameObject fromGO,bool setTO)
    {
        AILerp aiLerp = fromGO.GetComponent<AILerp>();
        aiLerp.enabled = setTO;
    }

    public void DestroyAllEnemies()
    {
        increaseEnabled = false;
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            spawnedTurrets[i].DestroyTurret();
        }
    }
}
