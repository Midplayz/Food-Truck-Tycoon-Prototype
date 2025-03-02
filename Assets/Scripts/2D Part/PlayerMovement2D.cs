using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb;
    private Vector2 movement;

    public bool canMove = false;
    private Vector2 initialPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPos = gameObject.transform.position;
    }

    void Update()
    {
        if(canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); 
            movement.y = Input.GetAxisRaw("Vertical");   

            movement = movement.normalized;
        }
        else
        {
            movement = Vector2.zero; 
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; 
        }
    }

    public void OnSwitched()
    {
        canMove = true;
        transform.position = initialPos;
        movement = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }
}
