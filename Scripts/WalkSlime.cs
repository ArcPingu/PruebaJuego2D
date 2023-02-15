using UnityEngine;

public class WalkSlime : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float jumpDuration = 0.5f;
    private float jumpTime;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private bool isJumping;
    private bool isGrounded; // Variable adicional para controlar si el personaje está en el suelo o no

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        isJumping = false;
        isGrounded = false; // Al inicio, el personaje no está en el suelo
    }


    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && isGrounded) // Solo permitir salto si el personaje está en el suelo y no está saltando actualmente
        {
            isJumping = true;
            jumpTime = Time.time;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (Time.time - jumpTime < jumpDuration)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                isJumping = false;
            }
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);

        rb.AddForce(movement * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.velocity = Vector3.zero;
            isJumping = false; // Si el personaje aterriza en una plataforma, ya no está saltando
            isGrounded = true; // Si el personaje aterriza en una plataforma, está en el suelo
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false; // Si el personaje sale de una plataforma, ya no está en el suelo
        }
    }
}
