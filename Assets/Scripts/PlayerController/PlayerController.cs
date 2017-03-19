using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    public float jumpVelocity = 400f;
    public float jetpackForce = 500f;
    public float jumpTime = 1f;
    public float maxUpwardVelocity = 15f;
    public float maxDownwardVelocity = -20f;

    private Rigidbody rigidbody;
    private int fuel = 100;
    private bool grounded = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check to see if we can jump, and if we are jumping
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }

        // Jetpack!
        Jetpack();
    }

    private void FixedUpdate()
    {
        // Get user input for horizontal movement, turn that into a velocity, then apply it
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        Vector3 velocity = input * speed * Time.fixedDeltaTime;
        transform.Translate(velocity);

        // If we're on the ground, start refueling
        if (grounded)
        {
            fuel++;
        }

        // Apply gravity to the player
        if (!grounded)
        {
            rigidbody.AddForce(new Vector3(0f, -1500f, 0f) * Time.deltaTime);
        }
        else if (grounded)
        {
            rigidbody.AddForce(new Vector3(0f, -500f, 0f) * Time.deltaTime);
        }

        CheckVelocities();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we're on the ground, we're grounded
        if (collision.collider.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If we leave the ground, we aren't grounded
        if (collision.collider.tag == "Ground")
        {
            grounded = false;
        }
    }

    private IEnumerator Jump()
    {
        float time = 0f;

        // Check if we're still trying to jump and if we've exceeded the jump time limit
        while (Input.GetKey(KeyCode.Space) && time <= jumpTime)
        {
            // Apply the velocity, inverse lerped between 0 and the max jump time for smoother jumping
            float jumpLerp = 1 - Mathf.InverseLerp(0f, jumpTime, time);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpVelocity * jumpLerp, rigidbody.velocity.z);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void Jetpack()
    {
        // Check to see if we are using the jetpack, and if we have enough fuel to keep using it
        if (Input.GetKey(KeyCode.W) && fuel > 0)
        {
            // Burn fuel and accelerate upwards
            fuel--;
            rigidbody.AddForce(new Vector3(0f, jetpackForce, 0f) * Time.deltaTime);

            
        }
    }

    private void CheckVelocities()
    {
        // Check to see if we've exceeded the max vertical velocity
        if (rigidbody.velocity.y > maxUpwardVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxUpwardVelocity, rigidbody.velocity.z);
        }

        if (rigidbody.velocity.y < maxDownwardVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxDownwardVelocity, rigidbody.velocity.z);
        }
    }
}
