using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool doubleJump;
    private bool isBlowing;

    private Rigidbody2D rig;
    private Animator anim;

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    public AudioClip jumpSound;
    public AudioClip GameOver;
    public AudioClip BlowingSound;
    private AudioSource audioSource;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
    
    void Update()
    {
        Move();

        // Checa se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Atualiza animação de pulo
        anim.SetBool("jump", !isGrounded);

        // Checa input de pulo
        if (inputActions.Player.Jump.WasPerformedThisFrame())
        {
            Jump();
        }
    }

    void Move()
    {
        rig.linearVelocity = new Vector2(moveInput.x * speed, rig.linearVelocity.y);

        if (moveInput.x > 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (moveInput.x < 0f)
        {
            anim.SetBool("run", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            anim.SetBool("run", false);
        }
    }

    void Jump()
    {
        // trava pulo se estiver no vento da fan
        if (isBlowing) return;

        if (isGrounded) // Primeiro pulo
        {
            audioSource.PlayOneShot(jumpSound);
            rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            doubleJump = true;
            anim.SetBool("jump", true);

        }
        else if (doubleJump) // Segundo pulo
        {
            audioSource.PlayOneShot(jumpSound);
            rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            doubleJump = false;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Saw"))
        {
            AudioSource.PlayClipAtPoint(GameOver, Camera.main.transform.position, 1f);
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 11)
        {
            if (!isBlowing) // só reseta 1x quando entrar
            {
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0f);
            }
            isBlowing = true;
            AudioSource.PlayClipAtPoint(BlowingSound, Camera.main.transform.position, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 11)
        {
            isBlowing = false;
        }
    }
}
