using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ProjectileGun : MonoBehaviour
{
    [Header("Bullet Stuff")]
    public GameObject bullet;
    int bulletsLeft, bulletsShot;

    [Header("Forces")]
    public float shootForce, upwardForce;

    [Header("Customization")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    [Header("Recoil")]
    public GunRecoil gunRecoilSettings = new GunRecoil();
    public float recoilForce;

    [Header("Display")]
    public ParticleSystem muzzleFlash;
    public TextMeshPro ammunitionDisplay;

    [Header("Actions")]
    bool shooting, readyToShoot, reloading;
    public bool allowInvoke = true;

    [Header("References")]
    public Rigidbody rb;
    public Camera fpsCam;
    public Transform attackPoint;
    

    private void Awake() { 
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);

        gunRecoilSettings.Update();
        transform.localPosition = gunRecoilSettings.RecoilOffsetPos;

        Quaternion swayRotation = GetComponent<WeaponSway>().SwayRotation;
        Quaternion recoilRotation = Quaternion.Euler(gunRecoilSettings.RecoilOffsetRot);

        transform.localRotation = swayRotation * recoilRotation;
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {
            targetPoint = hit.point;

            EnemyAI enemyAI = hit.collider.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Debug.Log("Hit an enemy!");
                enemyAI.TakeDamage(10); // Apply 10 damage
            }
        }
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread,spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, attackPoint.rotation);
        Debug.DrawLine(attackPoint.position, attackPoint.position + directionWithSpread.normalized * 10f, Color.red, 1f);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            muzzleFlash.Play();

        bulletsLeft--;
        bulletsShot++;

        gunRecoilSettings.AddRecoil();

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            rb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);

    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

[System.Serializable]
public class GunRecoil
{
    [Header("Variables")]
    [SerializeField] private float recoilAmount;
    [SerializeField] private float recoilAimRatio;
    [SerializeField] private float dampingRatio;
    [SerializeField] private float angularFrequency;

    [SerializeField] private Vector3 recoilOffsetPos = Vector3.zero;
    [SerializeField] private Vector3 recoilOffsetRot = Vector3.zero;

    public Vector3 RecoilOffsetPos { get; private set; } = Vector3.zero;
    public Vector3 RecoilOffsetRot { get; private set; } = Vector3.zero;

    private Vector3 desiredRecoilOffsetPos = Vector3.zero;
    private Vector3 desiredRecoilOffsetRot = Vector3.zero;

    private Vector3 recoilOffsetPosVel = Vector3.zero;
    private Vector3 recoilOffsetRotVel = Vector3.zero;

    public void AddRecoil()
    {
        desiredRecoilOffsetPos = recoilOffsetPos;
        desiredRecoilOffsetRot = recoilOffsetRot;

        desiredRecoilOffsetRot.z *= UnityEngine.Random.Range(1f, -1f);
    }

    public void Update()
    {
        desiredRecoilOffsetPos = Vector3.Lerp(desiredRecoilOffsetPos, Vector3.zero, 15f * Time.deltaTime);
        desiredRecoilOffsetRot = Vector3.Lerp(desiredRecoilOffsetRot, Vector3.zero, 15f * Time.deltaTime);

        Vector3 recoilDeltaPos = recoilAmount * desiredRecoilOffsetPos;
        Vector3 recoilDeltaRot = recoilAmount * desiredRecoilOffsetRot;

        Vector3 smoothRecoilPos = RecoilOffsetPos;
        Vector3 smoothRecoilRot = RecoilOffsetRot;

        HarmonicMotion.Calculate(ref smoothRecoilPos, ref recoilOffsetPosVel, recoilDeltaPos,
            HarmonicMotion.CalcDampedSpringMotionParams(dampingRatio, angularFrequency));
        RecoilOffsetPos = smoothRecoilPos;

        HarmonicMotion.Calculate(ref smoothRecoilRot, ref recoilOffsetRotVel, recoilDeltaRot,
            HarmonicMotion.CalcDampedSpringMotionParams(dampingRatio, angularFrequency));
        RecoilOffsetRot = smoothRecoilRot;
    }
}
