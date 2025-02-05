using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] bool isSprinting;
    [SerializeField] Vector2 moveDir;

    Rigidbody2D rb2d;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(moveDir);
    }

    void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        Debug.Log(value.isPressed);
        if(moveDir != Vector2.zero)
        {
            //Debug.Log("Sprinting");
            isSprinting = true;
        }
    }

    void Move(Vector2 moveDirection)
    {
        if(PlayerState.currentState != PlayerState.PlayerStates.Playing)
        {
            return;
        }

        if (isSprinting)
        {
            rb2d.linearVelocity = moveDirection * sprintSpeed;
        }
        else
        {
            rb2d.linearVelocity = moveDirection * moveSpeed;
        }
    }
}
