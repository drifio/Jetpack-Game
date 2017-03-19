using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    public float jumpForce = 1000f;
    public float jetpackForce = 500f;

    private Rigidbody rb;
    private int fuel = 100;

    [HideInInspector]
    public bool jump = false;
    [HideInInspector]
    public bool grounded = true;
    [HideInInspector]
    public bool jetpackActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !grounded)
        {
            jetpackActive = true;
        }
        if (grounded)
        {
            Jump();
        }
        else if (jetpackActive)
        {
            Jetpack();
        }
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

    private void Jump()
    {
        jetpackActive = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
        }
    }

    private void Jetpack()
    {
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            fuel--;
            rb.AddForce(new Vector3(0f, jetpackForce, 0f));
        }
    }
}
