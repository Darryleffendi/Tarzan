using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileFactory : MonoBehaviour, IFactory
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform parent;

    public Object Create(Vector3 spawnpoint)
    {
        GameObject obj = Instantiate(prefab, spawnpoint, transform.rotation, parent);
        Crocodile croc = obj.GetComponent<Crocodile>();
        croc.SetNest(GetComponent<AnimalNest>());
        Transform body = obj.transform.Find("crocodile_25659");
        body.Rotate(0, -90, 0);
        return croc;
    }
}
