using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KFCScript : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 10f;
    [SerializeField]
    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera <= detectionRange && FindObjectsOfType<Enemy>().Length == 0)
        {
            Vector3 directionToCamera = Camera.main.transform.position - textMeshPro.transform.position;
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);
            rotationToCamera *= Quaternion.Euler(0, 180, 0);
            textMeshPro.transform.rotation = rotationToCamera;
            textMeshPro.gameObject.SetActive(true);
        }
        else
        {
            textMeshPro.gameObject.SetActive(false);
        }

    }
}
