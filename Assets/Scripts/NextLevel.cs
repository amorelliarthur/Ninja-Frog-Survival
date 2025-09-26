// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class NextLevel : MonoBehaviour
// {
//     public string LvlName;
//     public int requiredStrawberries = 10; // quantidade necessária para liberar o próximo nível

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Player"))
//         {
//             // Checa se o total de morangos coletados já chegou no mínimo necessário
//             if (GameController.instance.totalScore >= requiredStrawberries)
//             {
//                 SceneManager.LoadScene(LvlName);
//             }
//             else
//             {
//                 // Debug.Log("Ainda faltam morangos!");                 
//             }
//         }
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevel : MonoBehaviour
{
    public string LvlName;
    public int requiredStrawberries = 10;
    public TextMeshProUGUI warningText; // arraste o texto no inspector

    public AudioClip GameWin;
    private AudioSource audioSource;

    void Start()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false); // começa desativado
        }
        audioSource = GetComponent<AudioSource>();
            
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameController.instance.totalScore >= requiredStrawberries)
            {
                AudioSource audio = new GameObject("TempAudio").AddComponent<AudioSource>();
                audio.clip = GameWin;
                audio.Play();
                DontDestroyOnLoad(audio.gameObject);

                
                AudioSource.PlayClipAtPoint(GameWin, Camera.main.transform.position, 1f);
                SceneManager.LoadScene(LvlName);
                

            }
            else
            {
                ShowWarning();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HideWarning();
        }
    }

    void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.text = $"REQUIRED {requiredStrawberries} STRAWBERRIES!";
            warningText.gameObject.SetActive(true);
        }
    }

    void HideWarning()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }
}
