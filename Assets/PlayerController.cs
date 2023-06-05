using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float collisionOffset = 0.05f;
    [SerializeField] public ContactFilter2D movementFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        //Ugly af, IDGAF!!
        if (movementInput != Vector2.zero)
        {
            if (TryMove(movementInput) == false)
            {
                if (TryMove(new Vector2(movementInput.x, 0))) return;

                if (TryMove(new Vector2(0, movementInput.y))) return;
            }
        }
        animator.SetBool("IsMoving", true);
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
                );

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
