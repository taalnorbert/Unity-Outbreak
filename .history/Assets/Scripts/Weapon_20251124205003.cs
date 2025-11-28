using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Publikus fegyver statisztikák
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f; // Mennyi lövés másodpercenként (1.0 = 1 lövés/mp)
    public int maxAmmo = 30; // Tárkapacitás
    public int totalAmmo = 120; // Teljes lőszerkészlet
    
    // Privát állapotok
    [HideInInspector] public int currentAmmo; // Aktuális lőszer a tárban
    private float nextTimeToFire = 0f; // Következő lehetséges lövés időpontja

    // Hivatkozások
    public Camera fpsCam; // A First Person kamera (amiből a Raycast indul)
    public ParticleSystem muzzleFlash; // Tűzcsóva effekt
    public GameObject impactEffect; // Becsapódás effekt (opcionális)
    private AudioSource audioSource;
    
    void Start()
    {
        currentAmmo = maxAmmo;
        // Az FPS kamera megtalálása a Hierarchy-ban
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
    }

    void Update()
    {
        // Lövés input kezelése
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        } 
        else if (Input.GetButton("Fire1") && currentAmmo <= 0)
        {
            // Ide jönne a 'klikkelő hang' (később)
        }

        // Újratöltés input kezelése
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