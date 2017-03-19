using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public int health = 3;

    private float iTime = 1.5f;
    private bool invincible = false;

    private void Start()
    {
        Debug.Log(health);
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Check to see if we've collided with a collectable and, if so, destroy it and increment the score
        if (collider.tag == "Collectable")
        {
            Destroy(collider.gameObject);
            score++;
            Debug.Log("Score is: " + score);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // Check to see if we've collied with something that hurts and, if so, take damage
        if (collider.tag == "Harm" && !invincible)
        {
            StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage()
    {
        health--;
        Debug.Log(health);
        float time = 0f;
        invincible = true;

        while (time <= iTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        invincible = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
