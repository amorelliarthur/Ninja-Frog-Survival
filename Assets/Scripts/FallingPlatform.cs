// using UnityEngine;

// public class FallingPlatform : MonoBehaviour
// {
//     public float fallingTime;
//     private TargetJoint2D target;
//     private BoxCollider2D boxCollider;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         target = GetComponent<TargetJoint2D>();
//         boxCollider = GetComponent<BoxCollider2D>();
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.tag == "Player")
//         {
//             Invoke("Falling", fallingTime);
//         }

        
//     }

//     void OnTriggerEnter2D(Collider2D collider)
//     {
//         if (collider.gameObject.layer == 9)
//         {
//             Destroy(gameObject);
//         }
//     }

//     void Falling()
//     {
//         target.enabled = false;
//         boxCollider.isTrigger = true;
//     }
// }

using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallingTime = 1f;   // tempo até cair
    public float respawnTime = 2f;   // tempo até reaparecer

    private TargetJoint2D target;
    private BoxCollider2D boxCollider;
    private Vector3 startPosition;   // posição inicial
    private Quaternion startRotation; // rotação inicial
    private Rigidbody2D rb;
    public AudioClip FallingSound;
    private AudioSource audioSource;
    private Rigidbody2D playerRb;

    void Start()
    {
        target = GetComponent<TargetJoint2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        audioSource = GetComponent<AudioSource>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        Invoke(nameof(Falling), fallingTime);
    }
}

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Invoke(nameof(Falling), fallingTime);
    //     }
    // }

    // void Falling()
    // {
    //     // Pega o gravityScale do Player
    //     Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    //     rb.gravityScale = playerRb.gravityScale;

    //     target.enabled = false;
    //     boxCollider.isTrigger = true;

    //     Invoke(nameof(Respawn), respawnTime);
    //     AudioSource.PlayClipAtPoint(FallingSound, transform.position, 1f);
    // }
    // void Falling()
    // {
    //     // Debug para identificar o que está null
    //     if (rb == null) Debug.LogError("rb está NULL");
    //     if (target == null) Debug.LogError("target está NULL");
    //     if (boxCollider == null) Debug.LogError("boxCollider está NULL");
    //     if (FallingSound == null) Debug.LogError("FallingSound está NULL");

    //     GameObject player = GameObject.FindGameObjectWithTag("Player");
    //     if (player == null) 
    //     {
    //         Debug.LogError("Não achei nenhum objeto com a tag Player!");
    //         return;
    //     }

    //     Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
    //     if (playerRb == null)
    //     {
    //         Debug.LogError("O objeto Player não tem Rigidbody2D!");
    //         return;
    //     }

    //     rb.gravityScale = playerRb.gravityScale;

    //     target.enabled = false;
    //     boxCollider.isTrigger = true;

    //     Invoke(nameof(Respawn), respawnTime);
    //     AudioSource.PlayClipAtPoint(FallingSound, transform.position, 1f);
    // }

    void Falling()
{
    if (playerRb != null)
        rb.gravityScale = playerRb.gravityScale;
    else
        rb.gravityScale = 2f; // fallback caso não encontre o player

    target.enabled = false;
    boxCollider.isTrigger = true;

    Invoke(nameof(Respawn), respawnTime);
    AudioSource.PlayClipAtPoint(FallingSound, transform.position, 1f);
}

    void Respawn()
    {
        // Reseta posição e estado
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPosition;
        transform.rotation = startRotation;

        target.enabled = true;
        boxCollider.isTrigger = false;
        rb.gravityScale = 0f; // fica "preso" até alguém pisar de novo
    }
}
