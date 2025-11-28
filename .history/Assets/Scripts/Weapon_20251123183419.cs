using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Inspector beallitasok
    public float fireRate = 0.2f; // Tűzgyorsaság: 0.2 másodpercenként lő (5 lövés/másodperc)
    public int damage = 25;      // Egy lövés sebzése
    public int magazine = 12;    // Tárkapacitás
    public float range = 100f;   // Lőtávolság
    
    // Kotelezo huzd-ra objektumok
    public Transform muzzle;     // A cső vége (MuzzlePoint)
    public LayerMask hitMask;    // Melyik réteget találja el a lövés (csak Enemy)

    // Privat valtozok
    private float lastFire;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = magazine;
        lastFire = Time.time;
    }

   void Update()
    {
        // 1. Looves (bal egergomb)
        if(Input.GetMouseButton(0) && Time.time - lastFire >= fireRate && currentAmmo > 0)
        {
            Shoot();
            lastFire = Time.time;
        }

        // 2. Ujratoltes (R gomb)
        if(Input.GetKeyDown(KeyCode.R)) Reload();
    }

    void Shoot()
    {
        currentAmmo--;
        
        Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.red, 1f); 
        
        RaycastHit hit;
        if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, hitMask))
        {
            // Ellenőrizd a találatot
            var damageable = hit.collider.GetComponent<IDamageable>();
            if(damageable != null) 
            {
                damageable.TakeDamage(damage);
                // 💥 Ez a log jelzi a találatot
                Debug.Log("Találat: " + hit.collider.name + " sebzés: " + damage); 
            }
            // Ha nem IDamageable, de van találat
            else
            {
                 Debug.Log("Találat: " + hit.collider.name + " - NEM sérülékeny");
            }
        }
    }

    void Reload()
    {
        // MVP (Minimum Viable Product): azonnali újratöltés
        currentAmmo = magazine;
        Debug.Log("Újratöltve!");
    }
}