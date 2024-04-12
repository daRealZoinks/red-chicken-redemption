using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponentInChildren<Gun>().OnAmmoChanged += UpdateAmmoDisplay;
    }

    private void UpdateAmmoDisplay(int bulletsLeft, int magazineSize)
    {
        ammoDisplay.text = bulletsLeft + " / " + magazineSize;
    }
}