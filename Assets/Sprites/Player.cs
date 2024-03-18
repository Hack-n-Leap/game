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
        if (Input.GetKey(KeyCode.D)) { // la touche D permet de se déplacer  vers la droite 
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal + 1, moveVertical);
            rb.velocity = movement * speed;

            anim.SetBool("run", true);

        } else if (Input.GetKey(KeyCode.A)) { // La touche Q permet de déplacer vers la gauche
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal - 1, moveVertical);
            rb.velocity = movement * speed;

            anim.SetBool("run", true);

            
        } else {
            anim.SetBool("run", false);
        }

    }
}
