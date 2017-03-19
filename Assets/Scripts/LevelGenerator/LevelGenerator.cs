﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public LevelSet LevelSet;
    public bool RandomizeSeedOnStart = true;
    public int TargetComponents = 10;
    public int seed = 0;

    private Vector3 componentSpawnLocation = new Vector3(0f, 0f, 0f);
    private int componentCount;
    private List<LevelComponent> levelComponents;

    // Use this for initialization
    void Start () {
        if (RandomizeSeedOnStart)
        {
            seed = Random.Range(0, int.MaxValue);
        }

        Random.InitState(seed);
        componentCount = 0;
        levelComponents = new List<LevelComponent>();

        StartGeneration();
    }

    private void StartGeneration()
    {
        int spawn = Random.Range(0, LevelSet.SpawnComponent.Count);

        // Create the spawn room
        GameObject spawnRoom = Instantiate(LevelSet.SpawnComponent[spawn].gameObject, componentSpawnLocation, Quaternion.identity);
        // Add the height of the room to the spawn location
        componentSpawnLocation += new Vector3(0f, 29f, 0f);
        LevelComponent levelComponent = spawnRoom.GetComponent<LevelComponent>();

        // Add the room to the component list
        levelComponents.Add(levelComponent);
        componentCount++;

        // Until the Target number of components is reached, generate new components
        while(componentCount < TargetComponents)
        {
            StartCoroutine(GenerateNextComponent());
        }
    }


    private IEnumerator GenerateNextComponent()
    {
        // Select a random component from the set
        GameObject newComponent = Instantiate(LevelSet.CommonComponents[Random.Range(0, LevelSet.CommonComponents.Count)].gameObject, componentSpawnLocation, Quaternion.identity);
        componentSpawnLocation += new Vector3(0f, 29f, 0f);
        LevelComponent levelComponent = newComponent.GetComponent<LevelComponent>();
        newComponent.name = "Level Component " + componentCount;
        levelComponents.Add(levelComponent);
        componentCount++;
        yield return null;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
