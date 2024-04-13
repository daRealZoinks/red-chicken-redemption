using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private bool allowInvoke = true;

    private int bulletsLeft;
    private int bulletsShot;

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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (!readyToShoot || reloading || bulletsLeft <= 0) return;

        bulletsShot = 0;
        Shoot();
        if (bulletsLeft <= 0)
        {
            Reload();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        shootSound.Play();

        var ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        var targetPoint = Physics.Raycast(ray, out var hit) ? hit.point : ray.GetPoint(75);

        var direction = targetPoint - attackPoint.position;

        var currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;

        var currentBulletRigidbody = currentBullet.GetComponent<Rigidbody>();

        var force = direction.normalized * shootForce + fpsCam.transform.up * upwardForce;

        currentBulletRigidbody.AddForce(force, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;
        OnAmmoChanged?.Invoke(bulletsLeft, magazineSize);

        animator.SetTrigger("RECOIL");

        var flash = Instantiate(muzzleFlash, attackPoint);
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