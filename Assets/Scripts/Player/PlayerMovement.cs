using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 2f; // Vitesse de déplacement du joueur
    public float playerJumpForce = 5f; // Force de saut du joueur

    public Transform feet;
    private float feetRadius = 0.3f;
    public LayerMask collisionLayer;
    private GameManager gameManager;


    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(feet.position, feetRadius, collisionLayer);
        float moveHorizontal = 0;

        if (gameManager.gameData.playerUnlockedFunctions[0] && !gameManager.gameData.playerUnlockedFunctions[1]) {
            moveHorizontal = 2;
        }
        
        if (Input.GetKey(gameManager.gameData.playerFunctionsKey[1]) && gameManager.gameData.playerUnlockedFunctions[1]) { // la touche D permet de se déplacer  vers la droite
            moveHorizontal += 2; 
            sr.flipX = false;
        } else if (Input.GetKey(gameManager.gameData.playerFunctionsKey[2]) && gameManager.gameData.playerUnlockedFunctions[2]) { // La touche Q permet de déplacer vers la gauche
            moveHorizontal -= 2;
            sr.flipX = true;            
        }
        
        if (Input.GetKey(gameManager.gameData.playerFunctionsKey[3]) && gameManager.gameData.playerUnlockedFunctions[3] && grounded) { // La touche espace permet de sauter
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
