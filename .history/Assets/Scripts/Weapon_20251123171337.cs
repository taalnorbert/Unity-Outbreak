using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Inspector beallitasok
    public float fireRate = 0.2f; 
    public int damage = 25;
    public int magazine = 12;
    public float range = 100f;
    
    // Kotelezo huzd-ra objektumok
    public Transform muzzle;
    public LayerMask hitMask;

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
        
        // Raycast (hitscan) inditas
        RaycastHit hit;
        if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, range, hitMask))
        {
            // Ellenorizzuk, hogy az eltalalt objektumon van-e IDamageable interface
            var damageable = hit.collider.GetComponent<IDamageable>();
            if(damageable != null) 
            {
                damageable.TakeDamage(damage);
                Debug.Log("Találat: " + hit.collider.name + " sebzés: " + damage);
            }

            // (ide jon majd a VFX)
        }
    }

    void Reload()
    {
        // MVP: azonnali ujratoltes
        currentAmmo = magazine;
    }
}