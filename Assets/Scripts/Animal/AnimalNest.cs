using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalNest : MonoBehaviour
{
    /* === Mediator between player and animal ===*/

    [SerializeField]
    private Transform[] points;
    protected List<Animal> animals = new List<Animal>();

    private void Start()
    {
        for(int i = 0; i < 5; i ++)
        {
            Spawn();
        }
    }

    public abstract void Spawn();

    public void PlayerEnter(Player player)
    {
        foreach(Animal animal in animals)
        {
            if (animal.GetState() is Animal_IdleState state)
            {
                state.SetTarget(player);
            }
        }
    }

    public void PlayerLeave()
    {
        foreach (Animal animal in animals)
        {
            if (animal.GetState().GetType() == typeof(Animal_ChaseState))
                ((Animal_ChaseState)animal.GetState()).Next();

            if (animal.GetState().GetType() == typeof(Animal_IdleState))
                ((Animal_IdleState)animal.GetState()).RemoveTarget();
        }
    }

    public Vector3 RandomPoint()
    {
        int random = Mathf.FloorToInt(Random.Range(0f, points.Length));

        Vector3 pos = points[random].position;
        return pos;
    }

    public Vector3 RandomPoint(bool onTerrainSurface)
    {
        int random = Random.Range(0, points.Length);

        Vector3 pos = points[random].position;

        if(onTerrainSurface)
            pos.y = Terrain.activeTerrain.SampleHeight(points[random].position);
        return pos;
    }

    public List<Animal> GetAnimals()
    {
        return animals;
    }
}
