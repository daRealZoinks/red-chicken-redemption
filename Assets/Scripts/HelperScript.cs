using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelperScript : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private HelperAudioPlayer audioManager;

    [SerializeField] private Animator creditsAnimator;

    private Animator animator;
    private bool playerHasAlreadyReadHelpMessage = false;
    private bool playerHasAlreadyReadStoryMessage = false;
    private bool playerHasKilledAllCityBandits = false;
    private bool helperFinishedAnimation = false;
    private bool killedKFC = false;
    private bool playedAudioHelpMessage = false;
    private bool playedAudioStoryMessage = false;
    private bool playedAudioGoKillBandits = false;
    private bool playedAudioGoFindKFC = false;
    private bool playedKilledKFCMessage = false;

    // Messages
    private string helpMessage = "Oh no... Please, someone help!";

    private string storyMessage = "Dear lord! I think I can hear shootings coming from the Bar direction.\n" +
                                  "There were some strange looking guys roaming around today.\nBet it must be Kentucky F. Cornelius and his men again...\n" +
                                  "Please stop them, they are going to burn down the village!";

    private string killBanditsMessage = "What are you waiting for?\nDo something already!";

    private string goFindKFC =
        "You killed them all!\nBut Kentucky F. Cornelius exited the Bar right before you got in.\n" +
        "He is hiding somewhere in the city, near one of the horse stables.\n Go and punish him before he takes another life!";

    private string killedKFCMessage = "I can already feel the average body mass index decresing.\n" +
                                      "Great job!";

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CityAttack")
        {
            playedAudioHelpMessage = true;
            playedAudioStoryMessage = true;
            playedAudioGoKillBandits = true;
        }

        animator = GetComponent<Animator>();

        textMeshPro.fontSize = 18;
        textMeshPro.SetText(helpMessage);
        textMeshPro.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name == "City")
        {
            playedAudioHelpMessage = true;
            StartCoroutine(PlayHelpMessage());
        }
    }

    void Update()
    {
        Vector3 directionToCamera = Camera.main.transform.position - textMeshPro.gameObject.transform.position;
        Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);
        rotationToCamera *= Quaternion.Euler(0, 180, 0);
        textMeshPro.gameObject.transform.rotation = rotationToCamera;

        if (SceneManager.GetActiveScene().name == "City")
        {
            PlayCitySceneScript();
        }
        else if (SceneManager.GetActiveScene().name == "CityAttack" && FindObjectsOfType<Enemy>().Length == 0 &&
                 !playerHasKilledAllCityBandits)
        {
            playerHasKilledAllCityBandits = true;

            StartCoroutine(PlayCityAttackSceneScript());
        }

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= detectionRange &&
            SceneManager.GetActiveScene().name == "CityAttack" && helperFinishedAnimation)
        {
            textMeshPro.gameObject.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "CityAttack")
        {
            if (textMeshPro.gameObject.activeInHierarchy)
            {
                textMeshPro.gameObject.SetActive(false);
            }
        }

        if (FindObjectsOfType<KFCScript>().Length == 0 && !killedKFC &&
            SceneManager.GetActiveScene().name == "CityAttack")
        {
            killedKFC = true;
            textMeshPro.text = killedKFCMessage;
            StartCoroutine(PlayKilledKFCScript());
        }
    }

    void PlayCitySceneScript()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= detectionRange)
        {
            playerHasAlreadyReadHelpMessage = true;

            if (!playedAudioStoryMessage && audioManager.FinishedPlayingCurrentMessage())
            {
                playedAudioStoryMessage = true;
                audioManager.PlayStoryMessage();
            }

            if (playerHasAlreadyReadHelpMessage && !playerHasAlreadyReadStoryMessage)
            {
                textMeshPro.fontSize = 7;
                textMeshPro.SetText(storyMessage);
            }

            if (playerHasAlreadyReadStoryMessage)
            {
                textMeshPro.fontSize = 7;
                textMeshPro.SetText(killBanditsMessage);

                if (!playedAudioGoKillBandits && audioManager.FinishedPlayingCurrentMessage())
                {
                    playedAudioGoKillBandits = true;
                    audioManager.PlayGoKillBanditsMessage();
                }
            }

            if (!textMeshPro.gameObject.activeInHierarchy)
            {
                textMeshPro.gameObject.SetActive(true);
            }
        }
        else
        {
            if (textMeshPro.gameObject.activeInHierarchy)
            {
                textMeshPro.gameObject.SetActive(false);
            }

            if (playerHasAlreadyReadHelpMessage && !playerHasAlreadyReadStoryMessage)
            {
                playerHasAlreadyReadStoryMessage = true;
            }
        }

        if (!playerHasAlreadyReadHelpMessage)
        {
            textMeshPro.gameObject.SetActive(true);
        }
    }

    IEnumerator PlayCityAttackSceneScript()
    {
        animator.SetTrigger("KilledCityBandits");
        yield return new WaitForSeconds(8);
        textMeshPro.fontSize = 7;
        helperFinishedAnimation = true;


        textMeshPro.SetText(goFindKFC);
        if (!playedAudioGoFindKFC && audioManager.FinishedPlayingCurrentMessage())
        {
            playedAudioGoFindKFC = true;
            audioManager.PlayGoFindKFCAudio();
        }
    }

    IEnumerator PlayKilledKFCScript()
    {
        animator.SetTrigger("KilledKFC");
        textMeshPro.gameObject.SetActive(false);
        yield return new WaitForSeconds(9);

        textMeshPro.gameObject.SetActive(true);

        if (!playedKilledKFCMessage && audioManager.FinishedPlayingCurrentMessage())
        {
            playedKilledKFCMessage = true;
            audioManager.PlayKilledKFCAudio();
        }

        yield return new WaitForSeconds(7);

        creditsAnimator.SetTrigger("CreditsSlide");
    }

    IEnumerator PlayHelpMessage()
    {
        yield return new WaitForSeconds(2);
        audioManager.PlayHelpMessage();
    }
}