using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberjackFactory : MonoBehaviour, IFactory
{
    [SerializeField]
    private GameObject prefab1, prefab2, prefab3;

    public Object Create(Vector3 spawnpoint)
    {
        return Create();
    }

    public Object Create()
    {
        int prefabIndex = Random.Range(1, 4);

        int wave = TowerDefense.Instance.GetWave();

        GameObject prefab;
        int health;
        float speed;

        if (prefabIndex == 1)
        {
            prefab = prefab1;
            health = 100;
            speed = 2.5f;
        }
        else if (prefabIndex == 2)
        {
            prefab = prefab2;
            health = 200;
            speed = 1.3f;
        }
        else
        {
            prefab = prefab3;
            health = 150;
            speed = 1.9f;
        }

        Vector3[] path = MapGenerator.Instance.GetPath((NodeQuadrant)Random.Range(0, 4));
        GameObject obj = Instantiate(prefab, path[0], transform.rotation, transform);
        Lumberjack lumb = obj.GetComponent<Lumberjack>();
        lumb.SetAttributes(health + 5 * wave, speed + 0.25f * wave);
        lumb.SetPath(path);
        return lumb;
    }
}
