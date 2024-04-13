using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KFCScript : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 10f;

    private TextMeshPro textMeshPro;
    private GameObject textObject;

    private string kfcScript = "I am going to turn all of you into delicious fried chicken!";

    // Start is called before the first frame update
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
        textMeshPro.text = kfcScript;
        textMeshPro.fontSize = 5;
        textMeshPro.color = Color.white;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        // Hide the text initially
        textObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        Debug.Log("Distance to Camera: " + distanceToCamera);

        if (distanceToCamera <= detectionRange)
        {
            Debug.Log("Within detection range");
            if (FindObjectsOfType<Enemy>().Length == 0)
            {
                Debug.Log("No enemies found");
                textObject.SetActive(true);
                if (!textObject.activeInHierarchy)
                {
                    Vector3 directionToCamera = Camera.main.transform.position - textObject.transform.position;
                    Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);
                    rotationToCamera *= Quaternion.Euler(0, 180, 0);
                    textObject.transform.rotation = rotationToCamera;
                }
            }
        }
        else if (textObject.activeInHierarchy)
        {
            textObject.SetActive(false);
        }
    }
}
