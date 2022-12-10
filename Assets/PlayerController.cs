using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public ensures that the value can be edited in the Unity Editor without touching code
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate()
    {
        // Try to move if input is not zero 
        if(movementInput != Vector2.zero) {
            // Check for potential collisions before character is moving

            // Checking if the input/all key(s) I press point to a collision object 
            // Returning false if all keys I press would let my character move into a collision object
            // Returning true if at least one key I press would let my character move into free space
            if(!TryMove(movementInput)) {
                if(!TryMove(new Vector2(movementInput.x, 0))) {
                    if(!TryMove(new Vector2(0, movementInput.y))) {
                        animator.SetBool("isMoving", false);
                    } else {
                        animator.SetBool("isMoving", true);
                    }
                } else {
                    animator.SetBool("isMoving", true);
                }
            } else {
                animator.SetBool("isMoving", true);
            }

            // Reference to isMoving condition in animations switch in Unity Animator
            // Play player_walk animation when moving, play player_idle animation if not moving
        } else {
            animator.SetBool("isMoving", false);
        }

        // Set direction of sprite movmenet direction
        if (movementInput.x < 0) {
            spriteRenderer.flipX = true;
        } else if(movementInput.x > 0) {
            spriteRenderer.flipX = false;
        } 
    }

    private bool TryMove(Vector2 direction) {
        // i.e. If I'm pressing only the W key that would let my character move towards a collision object then the Vector2 is 0,0
        // That's because I'm getting the first false for the movement
        // 2nd it's checked if I can go in x direction -> I'm passing x = 0 for the movement and 0 for y as arguments (new Vector2(movementInput.x, 0))
        // So I'm passing 0,0 as a Vector2 which leads to false in else clause 
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset // The amount to cast equal to the movement plus an offset
            );

            // If there would be no collisions count is zero -> movement should take place
            if (count == 0) {
                // Time.fixedDeltaTime: The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed.
                // Multiplying Time.fixedDeltaTime ensures that movements are consistent, even if some frames are apllying faster or slower
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }

    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
}
