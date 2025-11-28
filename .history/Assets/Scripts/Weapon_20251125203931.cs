using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 30;
    public int totalAmmo = 120;
    public float reloadTime = 2.5f;
    
    [HideInInspector] public int currentAmmo;
    private float nextTimeToFire = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private AudioSource audioSource;
    public AudioSource reloadAudioSource;
    
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
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        } 
        else if (Input.GetButton("Fire1") && currentAmmo <= 0)
        {
        }

        if (Input.GetKeyDown(KeyCode.R))
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
                damageable.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
            }
        }
    }
    
    IEnumerator Reload()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToTake = Mathf.Min(ammoNeeded, totalAmmo);
        
        if (reloadAudioSource != null)
        {
            reloadAudioSource.Play();
        }

        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo += ammoToTake;
        totalAmmo -= ammoToTake;
        
        Debug.Log($"Újratöltve. Tár: {currentAmmo} / Készlet: {totalAmmo}");
    }
}