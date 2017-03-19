using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public List<GameObject> ObjectSet = new List<GameObject>();
    public bool RandomizeSeedOnStart = true;
    public int seed = 0;

    // Use this for initialization
    void Start () {
        if (RandomizeSeedOnStart)
        {
            seed = Random.Range(0, int.MaxValue);
        }

        Random.InitState(seed);

        SpawnObject();
    }

    void SpawnObject()
    {
        var gameObjectToInstantiate = ObjectSet[Random.Range(0, ObjectSet.Count)].gameObject;
        Instantiate(gameObjectToInstantiate, transform.position, gameObjectToInstantiate.transform.rotation);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

}
