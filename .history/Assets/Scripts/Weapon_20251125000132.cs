using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 30;
    public int totalAmmo = 120;
    
    [HideInInspector] public int currentAmmo;
    private float nextTimeToFire = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private AudioSource audioSource;
    
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
            Reload();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        
        // Tűzcsóva futtatása
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // HANG LEJÁTSZÁSA
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Raycast (Lövedék sugár)
        RaycastHit hit;
        
        // Lövés indítása a kamera közepéből
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Találat: " + hit.transform.name);

            // Megpróbáljuk elérni az IDamageable interfészt a találati objektumon
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            // Becsapódás effekt spawnolása (opcionális)
            if (impactEffect != null)
            {
                // Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
    
    void Reload()
    {
        // Kiszámoljuk, mennyi lőszert hiányzik a tárból
        int ammoNeeded = maxAmmo - currentAmmo;
        
        // Kiszámoljuk, mennyi lőszert tudunk ténylegesen kivenni a teljes készletből
        int ammoToTake = Mathf.Min(ammoNeeded, totalAmmo);
        
        // Újratöltjük a tárat
        currentAmmo += ammoToTake;
        
        // Levonjuk a teljes készletből
        totalAmmo -= ammoToTake;
        
        Debug.Log($"Újratöltve. Tár: {currentAmmo} / Készlet: {totalAmmo}");
    }
}