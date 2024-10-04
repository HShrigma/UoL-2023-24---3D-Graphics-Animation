using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPoints;
    [SerializeField] GameObject player;
    List<Transform> spawnTransforms;
    [SerializeField] GameObject slimePrefab;
    List<Enemy> enemies;
    int spawnCount;
    public UnityEvent OnSpawn;
    void Start()
    {
        enemies = new List<Enemy>();
        spawnTransforms = spawnPoints.GetComponentsInChildren<Transform>().ToList();
        spawnTransforms.RemoveAt(0);
        GenerateSpawns();
    }

    // Update is called once per frame
    void Update()
    {
        CleanSlimes();

        if (!enemies.Any())
        {
            int diffMult = Mathf.RoundToInt(Mathf.Clamp(GameManager.instance.Wave / 10, 0, spawnTransforms.Count - 1));
            GenerateSpawns(diffMult);
        }
    }

    void CleanSlimes()
    {
        if (enemies.Any(n => n == null))
        {
            enemies.RemoveAll(n => n == null);
        }
    }
    void GenerateSpawns(int difficultyMult = 0)
    {
        spawnCount = UnityEngine.Random.Range(1 + difficultyMult, spawnTransforms.Count + 1);
        List<int> spawnIndexes = GenerateSpawnIndexes();
        spawnIndexes.ForEach(index => enemies.Add(Instantiate(slimePrefab, spawnTransforms[index].position, Quaternion.identity).GetComponentInChildren<Enemy>()));
        foreach (Enemy enemy in enemies)
        {
            enemy.player = player;
            enemy.tier = UnityEngine.Random.Range(1, 4);
            enemy.rb.mass = enemy.tier;
        }
        OnSpawn.Invoke();
    }
    List<int> GenerateSpawnIndexes()
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < spawnCount; i++)
        {
            int toAdd = UnityEngine.Random.Range(0, spawnTransforms.Count);
            int compare = toAdd;
            while (indexes.Contains(toAdd))
            {
                toAdd++;
                if (toAdd == spawnTransforms.Count)
                {
                    toAdd = 0;
                }
                if (toAdd == compare)
                {
                    break;
                }
            }
            indexes.Add(toAdd);
        }
        return indexes;
    }

    public void OnSlimeDeathSpawn(GameObject slime)
    {
        if (slime.GetComponentInChildren<Enemy>().tier != 1)
        {
            Transform t = slime.GetComponentInChildren<Enemy>().gameObject.transform;
            t.position = new Vector3(t.position.x, 4f, t.position.z);
            enemies.Add(Instantiate(slime, t.position, Quaternion.identity).GetComponentInChildren<Enemy>());
            enemies.Add(Instantiate(slime, t.position, Quaternion.identity).GetComponentInChildren<Enemy>());
            enemies[enemies.Count - 1].tier -= 1;
            enemies[enemies.Count - 2].tier -= 1;
            enemies[enemies.Count - 1].PushInDir(t.right * -1);
            enemies[enemies.Count - 2].PushInDir(t.right);
        }
    }
}
