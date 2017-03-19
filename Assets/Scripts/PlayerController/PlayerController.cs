using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public float horizontalAcceleration = 500f;
    public float horizontalSpeed = 5f;
    public float maxJumpVelocity = 400f;
    public float jetpackForce = 500f;
    public float wallJumpForce = 250f;
    public float maxJumpTime = 1f;
    public float maxUpwardVelocity = 15f;
    public float maxDownwardVelocity = -20f;
    public int maxFuel = 100;
    public LayerMask groundMask;
    public Transform center;

    private Rigidbody rigidbody;
    private int fuel;
    private bool grounded = true;
    private bool wallJump = false;

    private void Awake()
    {
        fuel = maxFuel;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check to see if we can jump, and if we are jumping
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (wallJump)
            {
                StartCoroutine(WallJump());
            }
            else
            {
                StartCoroutine(Jump());
            }
        }

        // Jetpack!
        Jetpack();
    }

    private void FixedUpdate()
    {
        // Get user input for horizontal movement, turn that into a velocity, then apply it
        float h = Input.GetAxisRaw("Horizontal");
        //rigidbody.velocity = new Vector3(h * speed, rigidbody.velocity.y, rigidbody.velocity.z);
        rigidbody.AddForce(new Vector3(h * horizontalAcceleration, 0f, 0f));

        // If we're on the ground, start refueling
        if (grounded)
        {
            fuel = maxFuel;
        }

        // Wall jump check
        wallJump = OnWallCheck();

        // Apply gravity to the player
        rigidbody.AddForce(new Vector3(0f, -1500f, 0f) * Time.deltaTime);

        if (wallJump)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        }

        CheckVelocities();

        Debug.DrawRay(center.position, Vector3.left * transform.localScale.x / 2.5f, Color.green);
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
        while (Input.GetKey(KeyCode.Space) && time <= maxJumpTime)
        {
            // Apply the velocity, inverse lerped between 0 and the max jump time for smoother jumping
            float jumpLerp = 1 - Mathf.InverseLerp(0f, maxJumpTime, time);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxJumpVelocity * jumpLerp, rigidbody.velocity.z);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WallJump()
    {
        float time = 0f;

        if (Physics.Raycast(center.position, Vector3.left, transform.localScale.x, groundMask))
        {
            rigidbody.AddForce(new Vector3(wallJumpForce, 0f, 0f));
        }
        else if (Physics.Raycast(center.position, Vector3.right, transform.localScale.x, groundMask))
        {
            rigidbody.AddForce(new Vector3(-wallJumpForce, 0f, 0f));
        }

        // Check if we're still trying to jump and if we've exceeded the jump time limit
        while (Input.GetKey(KeyCode.Space) && time <= maxJumpTime)
        {
            // Apply the velocity, inverse lerped between 0 and the max jump time for smoother jumping
            float jumpLerp = 1 - Mathf.InverseLerp(0f, maxJumpTime, time);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxJumpVelocity * jumpLerp, rigidbody.velocity.z);
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
            rigidbody.AddForce(new Vector3(0f, jetpackForce, 0f));

            
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

        if (Mathf.Abs(rigidbody.velocity.x) > horizontalSpeed)
        {
            rigidbody.velocity = new Vector3(Mathf.Sign(rigidbody.velocity.x) * horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
        }
    }

    private bool OnWallCheck()
    {
        return (Physics.Raycast(center.position, Vector3.left, transform.localScale.x / 2.7f, groundMask) || Physics.Raycast(center.position, Vector3.right, transform.localScale.x / 2.7f, groundMask));
    }
}
