using UnityEngine;

public class Strawberry : MonoBehaviour
{
    private SpriteRenderer sr;
    private CapsuleCollider2D capsule;
    public GameObject collected;
    public int Score;
    public AudioClip collectedSound;
    private AudioSource audioSource;
    private bool isCollected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isCollected && collider.gameObject.CompareTag("Player"))
        {
            isCollected = true;

            sr.enabled = false;
            capsule.enabled = false;
            collected.SetActive(true);

            GameController.instance.totalScore += Score;
            GameController.instance.UpdateScoreText();

            AudioSource.PlayClipAtPoint(collectedSound, transform.position, 1f);

            Destroy(gameObject, 0.25f);
        }
    }
}
