using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    public float slowDownFactor = 0.3f;
    [SerializeField]
    public float slowDownLength = 0.1f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SlowDownTime();
        }

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(ResetTimeScale());
        }
    }

    void SlowDownTime()
    {
        Time.timeScale = slowDownFactor;
    }

    IEnumerator ResetTimeScale()
    {
        yield return new WaitForSeconds(slowDownLength);
        Time.timeScale = 1f;
    }
}
