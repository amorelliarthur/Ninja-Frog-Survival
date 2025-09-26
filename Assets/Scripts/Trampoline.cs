using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator anim;
    public float jumpForce;
    public AudioClip TrampolineSound;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created   
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         anim.SetTrigger("jump");
    //         collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    //         AudioSource.PlayClipAtPoint(TrampolineSound, Camera.main.transform.position, 1f);
    //     }

    // }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("jump");

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // zera a velocidade vertical
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            AudioSource.PlayClipAtPoint(TrampolineSound, Camera.main.transform.position, 1f);
        }
    }

}
