using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelperScript : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 10f;
    private GameObject textObject;
    private TextMeshPro textMeshPro;
    private bool playerHasAlreadyReadHelpMessage = false;
    private bool playerHasAlreadyReadStoryMessage = false;

    // Messages
    private string helpMessage = "Oh no... Please, someone help!";
    private string storyMessage = "Dear lord! I think I can hear shootings coming from the Bar direction.\n" +
        "There were some strange looking guys roaming around today.\nBet it must be Kentucky F. Cornelius and his men again...\n" +
        "Please stop them, they are going to burn the village down!";
    private string killBanditsMessage = "What are you waiting for?\nDo something already!";

    void Start()
    {
        textObject = new GameObject("CharacterText");
        textObject.transform.SetParent(transform, false);
        textObject.transform.localPosition = new Vector3(0, 2, 0);

        Canvas canvas = textObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        CanvasScaler canvasScaler = textObject.AddComponent<CanvasScaler>();
        canvasScaler.scaleFactor = 1f;

        GameObject textMesh = new GameObject("TextMesh");
        textMesh.transform.SetParent(textObject.transform, false);

        textMeshPro = textMesh.AddComponent<TextMeshPro>();
        textMeshPro.text = helpMessage;
        textMeshPro.fontSize = 5;
        textMeshPro.color = Color.white;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        // Hide the text initially
        textObject.SetActive(false);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= detectionRange)
        {
            playerHasAlreadyReadHelpMessage = true;

            if (playerHasAlreadyReadHelpMessage && !playerHasAlreadyReadStoryMessage)
            {
                textMeshPro.fontSize = 3;
                textMeshPro.SetText(storyMessage);
            }

            if (playerHasAlreadyReadStoryMessage)
            {
                textMeshPro.fontSize = 3;
                textMeshPro.SetText(killBanditsMessage);
            }

            if (!textObject.activeInHierarchy)
            {
                textObject.SetActive(true);
            }

            Vector3 directionToCamera = Camera.main.transform.position - textObject.transform.position;
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);
            rotationToCamera *= Quaternion.Euler(0, 180, 0);
            textObject.transform.rotation = rotationToCamera;
        }
        else
        {
            if (textObject.activeInHierarchy)
            {
                textObject.SetActive(false);
            }

            if (playerHasAlreadyReadHelpMessage && !playerHasAlreadyReadStoryMessage)
            {
                playerHasAlreadyReadStoryMessage = true;
            }
        }

        if (!playerHasAlreadyReadHelpMessage)
        {
            textObject.SetActive(true);
        }
    }
}
