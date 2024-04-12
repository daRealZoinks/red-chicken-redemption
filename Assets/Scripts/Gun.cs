using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour {
	public GameObject bullet, muzzleFlash;
	public TextMeshProUGUI ammunitionDisplay;
	public float shootForce, upwardForce, timeBetweenShooting, spread, reloadTime, timeBetweenShots;
	public int magazineSize, bulletsPerTap;
	public Camera fpsCam;
	public Transform attackPoint;
	public bool allowButtonHold, allowInvoke = true;
	int bulletsLeft, bulletsShot;

	bool shooting, readyToShoot, reloading;

	private Animator animator;

	private void Awake() {
		bulletsLeft = magazineSize;
		animator = GetComponent<Animator>();
		readyToShoot = true;
	}

	private void Update() {
		MyInput();
		if(ammunitionDisplay != null) {
			ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
		}
	}

	private void MyInput() {
		if(allowButtonHold)
			shooting = Input.GetKey(KeyCode.Mouse0);
		else
			shooting = Input.GetKeyDown(KeyCode.Mouse0);

		if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) {
			Reload();
		}
		//if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) {
		//	Reload();
		//}

		if(readyToShoot && shooting && !reloading && bulletsLeft > 0) {
			bulletsShot = 0;
			Shoot();
			if(bulletsLeft <= 0) {
				Reload();
			}
		}
	}

	private void Shoot() {
		readyToShoot = false;

		Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		Vector3 targetPoint;

		if(Physics.Raycast(ray, out hit)) {
			targetPoint = hit.point;
		}
		else {
			targetPoint = ray.GetPoint(75);
		}

		Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

		float x = Random.Range(-spread, spread);
		float y = Random.Range(-spread, spread);

		// Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
		GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
		currentBullet.transform.forward = directionWithoutSpread.normalized;

		currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
		currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

		if(muzzleFlash != null) {
			Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
		}

		--bulletsLeft;
		++bulletsShot;

		muzzleFlash.GetComponent<ParticleSystem>().Play();
		animator.SetTrigger("RECOIL");

		if(allowInvoke) {
			Invoke("ResetShot", timeBetweenShooting);
			allowInvoke = false;
		}

		if(bulletsShot < bulletsPerTap && bulletsLeft > 0) {
			Invoke("Shoot", timeBetweenShots);
		}
	}

	private void ResetShot() {
		readyToShoot = true;
		allowInvoke = true;
	}

	private void Reload() {
		reloading = true;
		Invoke("ReloadFinished", reloadTime);
	}

	private void ReloadFinished() {
		bulletsLeft = magazineSize;
		reloading = false;
	}
}
