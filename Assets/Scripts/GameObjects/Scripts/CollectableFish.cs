using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFish : MonoBehaviour {

    public float FloatStrength = 0.1f;

    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        var position = transform.position;
        position.y = originalPosition.y + Mathf.Sin(Time.time + transform.position.x) * FloatStrength;
        transform.position = position;
    }
}
