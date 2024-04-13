using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SalonDoor" && FindObjectsOfType<Enemy>().Length == 0)
        {
            SceneManager.LoadScene("Bar");
        } else if (collision.gameObject.tag == "SalonInteriorDoor" && FindObjectsOfType<Enemy>().Length == 0)
        {
            SceneManager.LoadScene("CityAttack");
        }
    }
}
