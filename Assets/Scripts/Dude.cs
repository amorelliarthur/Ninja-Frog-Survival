using Unity.VisualScripting;
using UnityEngine;

public class Dude : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    public float speed;
    public Transform rightCol;
    public Transform leftCol;
    public Transform headPoint;
    private bool colliding;
    public LayerMask layer;
    public BoxCollider2D boxCollider2D;
    public CircleCollider2D circleCollider2D;

    public AudioClip GameOver;
    public AudioClip DudeDead;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);
        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            speed *= -1f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {            
            float playerY = col.transform.position.y;
            float headY = headPoint.position.y;

            // if (playerY > headY) // só se o Player estiver acima da cabeça
            // {
            //     // Player pisou na cabeça -> inimigo morre
            //     col.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            //     speed = 0;
            //     anim.SetTrigger("die");
            //     boxCollider2D.enabled = false;
            //     circleCollider2D.enabled = false;
            //     rig.bodyType = RigidbodyType2D.Kinematic;
            //     Destroy(gameObject, 0.33f);
            // }
            if (playerY > headY) // só se o Player estiver acima da cabeça
            {
                // Player pisou na cabeça -> inimigo morre
                Rigidbody2D playerRb = col.gameObject.GetComponent<Rigidbody2D>();
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f); // zera a velocidade vertical
                playerRb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                speed = 0;
                anim.SetTrigger("die");
                boxCollider2D.enabled = false;
                circleCollider2D.enabled = false;
                rig.bodyType = RigidbodyType2D.Kinematic;
                audioSource.PlayOneShot(DudeDead);
                Destroy(gameObject, 0.33f);
            }
            else
            {

                GameController.instance.ShowGameOver();
                Destroy(col.gameObject);
                audioSource.PlayOneShot(GameOver);
            }
        }

    }
}
