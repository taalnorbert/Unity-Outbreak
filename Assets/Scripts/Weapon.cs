using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float minDamage = 10f;
    public float maxDamage = 25f;

    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 30;
    public int totalAmmo = 120;
    public float reloadTime = 3.0f;
    
    [HideInInspector] public int currentAmmo;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private AudioSource audioSource;
    public AudioSource reloadAudioSource;
    public Animator weaponAnimator;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0 && !isReloading)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        } 
        else if (Input.GetButton("Fire1") && currentAmmo <= 0)
        {
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        currentAmmo--;
        
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        RaycastHit hit;
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Találat: " + hit.transform.name);

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                float damage = Random.Range(minDamage, maxDamage);
                damageable.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
            }
        }
    }
    
    IEnumerator Reload()
    {
        isReloading = true;
        
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToTake = Mathf.Min(ammoNeeded, totalAmmo);
        
        if (weaponAnimator != null)
        {
            weaponAnimator.SetBool("IsReloading", true);
            
            Debug.Log("DEBUG RELOAD: IsReloading paraméter TRUE-ra állítva.");
            Debug.Log("DEBUG ANIMATOR STATE: " + weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pistol_Idle"));
        }
        
        if (reloadAudioSource != null)
        {
            reloadAudioSource.Play();
        }

        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo += ammoToTake;
        totalAmmo -= ammoToTake;
        
        Debug.Log($"Újratöltve. Tár: {currentAmmo} / Készlet: {totalAmmo}");
        
        if (weaponAnimator != null)
        {
            weaponAnimator.SetBool("IsReloading", false);
        }
        
        isReloading = false;
    }
}