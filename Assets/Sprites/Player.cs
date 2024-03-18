using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f; // Vitesse de déplacement du joueur

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.D)) { // la touche D permet de se déplacer  vers la droite 
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rb.velocity = movement * speed;

        } else if (Input.GetKey(KeyCode.A)) { // La touche Q permet de déplacer vers la gauche
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rb.velocity = movement * speed;
        }
        
        if (Input.GetKey(KeyCode.Space)) { // La touche espace permet de sauter
            moveVertical += 2;
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rb.velocity = movement * speed;
        } 

        anim.SetBool("run", moveHorizontal != 0);
        anim.SetBool("jump", moveVertical != 0);

    }
}
