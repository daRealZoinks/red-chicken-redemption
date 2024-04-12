using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponent<IDamageable>().OnHealthChanged+= UpdateHealthDisplay;
        playerObject.GetComponentInChildren<Gun>().OnAmmoChanged += UpdateAmmoDisplay;
    }

    private void UpdateHealthDisplay(int health)
    {
        healthDisplay.text = health.ToString();
    }

    private void UpdateAmmoDisplay(int bulletsLeft, int magazineSize)
    {
        ammoDisplay.text = bulletsLeft + " / " + magazineSize;
    }
}