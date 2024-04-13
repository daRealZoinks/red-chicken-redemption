using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    public GameObject muzzleFlash;
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float spread;
    [SerializeField] private float reloadTime;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int magazineSize;
    [SerializeField] private int bulletsPerTap;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private AudioSource[] shootSounds;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private bool allowButtonHold;
    [SerializeField] private bool allowInvoke = true;

    private int bulletsLeft;
    private int bulletsShot;

    private bool shooting;
    private bool readyToShoot = true;
    private bool reloading;

    private Camera fpsCam;

    private Animator animator;

    public Action<int, int> OnAmmoChanged; // bullets left / magazine size

    private void Awake()
    {
        bulletsLeft = magazineSize;
        OnAmmoChanged?.Invoke(bulletsLeft, magazineSize);
        animator = GetComponent<Animator>();
        fpsCam = Camera.main;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
            if (bulletsLeft <= 0)
            {
                Reload();
            }
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        var ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        var targetPoint = Physics.Raycast(ray, out var hit) ? hit.point : ray.GetPoint(75);

        var directionWithoutSpread = targetPoint - attackPoint.position;

        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);

        // Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        var currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        var currentBulletRigidbody = currentBullet.GetComponent<Rigidbody>();

        var force = directionWithoutSpread.normalized * shootForce + fpsCam.transform.up * upwardForce;

        currentBulletRigidbody.AddForce(force, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;
        OnAmmoChanged?.Invoke(bulletsLeft, magazineSize);

        shootSounds[Random.Range(0, shootSounds.Length)].Play();

        animator.SetTrigger("RECOIL");

        GameObject flash = Instantiate(muzzleFlash, attackPoint);
        Destroy(flash, 0.2f);

        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        animator.SetTrigger("RELOAD");
        reloadSound.Play();
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        OnAmmoChanged?.Invoke(bulletsLeft, magazineSize);
        reloading = false;
    }
}