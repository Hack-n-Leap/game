using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 2f; // Vitesse de déplacement du joueur
    public float playerJumpForce = 5f; // Force de saut du joueur

    public Transform feet;
    private float feetRadius = 0.3f;
    public LayerMask collisionLayer;


    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(feet.position,feetRadius,collisionLayer);
        float moveHorizontal = Input.GetAxis("Horizontal");
        

        if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) { // la touche D permet de se déplacer  vers la droite
            moveHorizontal += 1; 
            sr.flipX = false;
            

        } else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) { // La touche Q permet de déplacer vers la gauche
            moveHorizontal -= 1;
            sr.flipX = true;
            
            
        }
        
        if (Input.GetKey(KeyCode.Space) && grounded) { // La touche espace permet de sauter
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);

            grounded = false;

        }

        rb.velocity = new Vector2(moveHorizontal * playerSpeed, rb.velocity.y);

        anim.SetBool("run", moveHorizontal != 0);
        anim.SetBool("jump", !grounded);

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feet.position,feetRadius);
    }
}
