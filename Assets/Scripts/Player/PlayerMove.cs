using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // How much time the player can hang in the air
    private float coyoteCounter; // How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumps")]
    [SerializeField] private float wallJumpX;// Horizontal wall jump force
    [SerializeField] private float wallJumpY;// Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;


    private Rigidbody2D body;
    private Animator aim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    private float horizontalInput;

    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        aim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        jumpCounter = extraJumps;
    }

    // Update is called once per frame
    void Update()
    {
        // Run without attack
        if (!movementEnabled)
            return;

        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character when moving left or right
        if(horizontalInput > 0.01f)
            transform.localScale = new Vector3(10, 10, 10);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-10,10,10);  

        // Set animator
        aim.SetBool("run", horizontalInput != 0);
        aim.SetBool("grounded", IsGrounded());

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y >0 )
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (OnWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (IsGrounded())
            {
                coyoteCounter = coyoteTime; // Reset coyote counter when on the ground
                jumpCounter = extraJumps; // Reset jump counter to extra jump value
            }
            else
            {
                coyoteCounter = Time.deltaTime; // Start decreasing coyote counter when not on the ground
            }
        }
    }
    private void Jump()
    {
        // If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything
        if (coyoteCounter < 0 && !OnWall() && jumpCounter <= 0) return;
        SoundManager.instance.Playsound(jumpSound);

        if (OnWall())
            WallJump();
        else
        {
            if (IsGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                // If not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    body.velocity = new Vector2 (body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) // If we have extra jumps then jump and decrease the jump counter
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            // Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCoolDown = 0;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
    private bool movementEnabled = true;

    public void DisableMovement()
    {
        movementEnabled = false;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }
}
