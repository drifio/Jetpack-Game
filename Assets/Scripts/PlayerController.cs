using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    public float jumpVelocity = 400f;
    public float jetpackForce = 500f;
    public float jumpTime = 1f;
    public float maxJetVelocity = 1f;

    private Rigidbody rb;
    private int fuel = 100;
    private bool grounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }
        Jetpack();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        Vector3 velocity = input * speed * Time.fixedDeltaTime;

        transform.Translate(velocity);

        if (grounded)
        {
            fuel++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = false;
        }
    }

    private IEnumerator Jump()
    {
        float time = 0f;

        while (Input.GetKey(KeyCode.Space) && time <= jumpTime)
        {
            float jumpLerp = 1 - Mathf.InverseLerp(0f, jumpTime, time);
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity * jumpLerp, rb.velocity.z);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void Jetpack()
    {
        if (Input.GetKey(KeyCode.W) && fuel > 0)
        {
            fuel--;
            rb.AddForce(new Vector3(0f, jetpackForce, 0f) * Time.deltaTime);

            if (rb.velocity.y > maxJetVelocity)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxJetVelocity, rb.velocity.z);
            }
        }
    }
}
