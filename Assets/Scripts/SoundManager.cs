using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager Instance {
		get; set;
	}
	public AudioSource gunShootingSound;

	private void Awake() {
		if(Instance != null && Instance != this) {
			Destroy(gameObject);
		}
		else {
			Instance = this;
		}
	}
}
