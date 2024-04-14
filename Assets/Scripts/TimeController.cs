using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.3f;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private VolumeProfile volumeProfile;

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

        volumeProfile.TryGet<Vignette>(out var vignette);
        var targetVignette = slowMotion ? 0.5f : 0.25f;
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, smoothTime * Time.deltaTime);

        volumeProfile.TryGet<ChromaticAberration>(out var chromaticAberration);
        var targetChromaticAberration = slowMotion ? 1f : 0f;
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetChromaticAberration, smoothTime * Time.deltaTime);

        volumeProfile.TryGet<LensDistortion>(out var lensDistortion);
        var targetLensDistortion = slowMotion ? -0.5f : 0f;
        lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetLensDistortion, smoothTime * Time.deltaTime);
    }

    private void OnDisable()
    {
        volumeProfile.TryGet<Vignette>(out var vignette);
        vignette.intensity.value = 0.25f;
    }
}