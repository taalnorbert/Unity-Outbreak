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
    [HideInInspector] public int currentAmmo; 
    private float nextTimeToFire = 0f; 

    // Hivatkozások
    public Camera fpsCam; 
    public ParticleSystem muzzleFlash; 
    public GameObject impactEffect; 
    private AudioSource audioSource;

    [Header("Recoil Settings")]
    public Transform cameraRecoilTarget; // Ezt a PlayerCameraRoot-ra kell állítani!
    // Finomhangolt értékek a stabilabb érzethez:
    public float recoilAmount = 2.5f;      // Felfelé rángatás mértéke
    public float recoilSideMax = 0.5f;     // Maximális oldalirányú rángatás
    public float recoilSpeed = 15f;        // Gyors beérkezés
    public float recoilReturnSpeed = 7f;   // Kényelmes visszatérés

    private Vector3 currentRecoil = Vector3.zero; // Aktuális animált eltolás
    private Vector3 recoilTarget = Vector3.zero;  // Cél eltolás (lövéssel nő)
    
    void Start()
    {
        currentAmmo = maxAmmo;
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
        audioSource = GetComponent<AudioSource>();
        
        // Alapértelmezett beállítás a stabil működéshez
        if (cameraRecoilTarget != null)
        {
            cameraRecoilTarget.localRotation = Quaternion.identity;
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
            // Tár üres hangja (később)
        }

        // Újratöltés input kezelése
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
        // 🔴 FIGYELEM: A Recoil logika KIVÉVE lett innen!
    }

    // 🏆 PROFI MEGOLDÁS: A Recoil logika a LateUpdate-be kerül
    // A LateUpdate az Update és minden mozgás/forgás után fut le.
    void LateUpdate()
    {
        // Recoil Recovery Logika (kamera mozgatása)
        if (cameraRecoilTarget != null)
        {
            // 1. Simán közelítsük a currentRecoil-t a recoilTarget felé (felrángatás)
            currentRecoil = Vector3.Lerp(currentRecoil, recoilTarget, Time.deltaTime * recoilSpeed);

            // 2. Alkalmazzuk a recoil-t a kamerára
            // Ez a sor most AZ UTOLSÓ, ami a kamera forgatását állítja a képkockában.
            cameraRecoilTarget.localRotation = Quaternion.Euler(currentRecoil);

            // 3. Simán közelítsük a recoilTarget-et nullához (visszatérés)
            recoilTarget = Vector3.MoveTowards(recoilTarget, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            
            // 4. Reset: Megállás a nulla közelében a jitter elkerülésére.
            if (recoilTarget.magnitude < 0.001f)
            {
                recoilTarget = Vector3.zero;
                currentRecoil = Vector3.zero;
            }
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
                // Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        DoRecoil(); // Hívás a lövés után!
    }
    
    // Profi Recoil kalkuláció
    void DoRecoil()
    {
        // Vízszintes eltérés (a recoilSideMax limitálja az oldalirányú mozgást)
        float sideRecoil = Random.Range(-recoilSideMax, recoilSideMax);
        
        // Hozzáadjuk a Recoilt a célponthoz.
        recoilTarget += new Vector3(
            sideRecoil, 
            recoilAmount, // Felfelé rángatás 
            0f
        );

        // Max függőleges Recoil limit (ne menjen túl magasra)
        recoilTarget.y = Mathf.Clamp(recoilTarget.y, 0, recoilAmount * 3f);
    }
    
    void Reload()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToTake = Mathf.Min(ammoNeeded, totalAmmo);
        
        currentAmmo += ammoToTake;
        totalAmmo -= ammoToTake;
        
        Debug.Log($"Újratöltve. Tár: {currentAmmo} / Készlet: {totalAmmo}");
    }
}