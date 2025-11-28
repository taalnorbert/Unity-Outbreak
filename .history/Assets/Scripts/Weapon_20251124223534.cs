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

    [Header("Recoil Settings")]
    public Transform cameraRecoilTarget; // Ezt a PlayerCameraRoot-ra kell állítani az Editorban!
    public float recoilAmount = 2f;      // A maximális felfelé rángatás mértéke
    public float recoilSpeed = 10f;      // Mennyire gyorsan rángatódik fel
    public float recoilReturnSpeed = 5f; // Mennyire gyorsan tér vissza az eredeti pozícióba

    private Vector3 currentRecoil = Vector3.zero; // A kamera aktuális, animált pozíciója
    private Vector3 recoilTarget = Vector3.zero;  // A cél pozíció, ahova a rángatásnak kell mennie
    
    void Start()
    {
        currentAmmo = maxAmmo;
        // Az FPS kamera megtalálása a Hierarchy-ban
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Weapon.cs

// Weapon.cs

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

    // Recoil Recovery Logika (kamera mozgatása)
    if (cameraRecoilTarget != null)
    {
        // 🚨 FŐ JAVÍTÁS: Guard Clause (Őrző Záradék)
        // Ha a recoil célpont szinte nulla, azonnal állítsuk meg a mozgást.
        if (recoilTarget.magnitude < 0.001f)
        {
            recoilTarget = Vector3.zero;
            currentRecoil = Vector3.zero;
            
            // 🔴 EZ A KULCS: Kilépünk a Recoil logikából.
            // Ezzel engedjük, hogy a MouseLook script állítsa be a kamera forgását konfliktus nélkül!
            return; 
        }

        // 1. Simán közelítsük a currentRecoil-t a recoilTarget felé (felrángatás)
        currentRecoil = Vector3.Lerp(currentRecoil, recoilTarget, Time.deltaTime * recoilSpeed);

        // 2. Alkalmazzuk a recoil-t a kamerára
        cameraRecoilTarget.localRotation = Quaternion.Euler(currentRecoil);

        // 3. Simán közelítsük a recoilTarget-et nullához (visszatérés)
        recoilTarget = Vector3.MoveTowards(recoilTarget, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
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

        DoRecoil(); // <<< ÚJ SOR: Hívás a lövés után!
    }
    
    void DoRecoil()
    {
        // Célpont beállítása a recoilAmount alapján (felfelé és kissé oldalra)
        recoilTarget += new Vector3(
            // Kisebb oldalirányú rángás (randomitás)
            Random.Range(-recoilAmount * 0.2f, recoilAmount * 0.2f), 
            recoilAmount, // Felfelé rángatás (a lövés fő hatása)
            0f
        );

        // Korlátozzuk a maximális recoil-t, hogy ne mozduljon el túl messze
        recoilTarget.y = Mathf.Clamp(recoilTarget.y, 0, recoilAmount * 2f);
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