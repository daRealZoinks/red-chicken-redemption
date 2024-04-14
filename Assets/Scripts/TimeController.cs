using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float smoothTime = 0.3f;

    private bool slowMotion;

    public void OnSlowMotion(InputAction.CallbackContext context)
    {
        slowMotion = context.phase switch
        {
            InputActionPhase.Started => true,
            InputActionPhase.Canceled => false,
            _ => slowMotion
        };
    }

    private void Update()
    {
        var targetTimeScale = slowMotion ? slowDownFactor : 1f;
        var targetPitch = slowMotion ? slowDownFactor : 1f;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, smoothTime * Time.deltaTime);

        var f = audioMixer.GetFloat("MasterPitch", out var pitch) ? pitch : 1f;
        var value = Mathf.Lerp(f, targetPitch, smoothTime * Time.deltaTime);
        audioMixer.SetFloat("MasterPitch", value);
    }
}