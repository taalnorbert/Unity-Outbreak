using UnityEngine;
using UnityEngine.AI; // NavMeshAgent-hez

public class ZombieAI : MonoBehaviour, IDamageable
{
    // Publikus változók (Inspectorban állítható)
    public float maxHealth = 50f;
    public float currentHealth;
    public float chaseDistance = 15f; // Ezen belül kezdi el a követést
    public float lookRadius = 10f; // A látótávolság sugara
    public float damage = 10f; // A zombi ennyi sebzést okoz
    public float attackRate = 1f; // Mennyi időnként támad (pl. 1mp)
    private float nextAttackTime = 0f;

    // Privát hivatkozások
    private Transform target; // A játékos pozíciója
    private NavMeshAgent agent; // A Nav Mesh Agent komponens
    private Animator anim; // Az animáció vezérléséhez
    // HIBA JAVÍTVA: Osztályszintű deklaráció a WaveManager-hez
    private WaveManager waveManager; 

    // Kezdeti beállítások
    void Start()
    {
        currentHealth = maxHealth;
        
        // Komponensek és célpontok keresése
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // HIBA JAVÍTVA: A WaveManager keresése elavult metódus nélkül
        // Megkeressük a WaveManager objektumot is a jelenetben:
        waveManager = FindFirstObjectByType<WaveManager>();
    }

    // Minden frame-ben lefut
    void Update()
    {
        // Távolság kiszámítása a játékos és a zombi között
        float distance = Vector3.Distance(target.position, transform.position);

        // Zombi sebesség átadása az Animátornak
        float speed = agent.velocity.magnitude / agent.speed; // Sebesség kiszámítása (0-1)
        if (anim != null)
        {
            anim.SetFloat("Speed", speed); // Átadja a Speed paraméternek az értéket
        }

        // Ha a játékos túl közel van (chaseDistance)
        if (distance <= chaseDistance)
        {
            // Követés
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Támadás Logikája
                if (Time.time >= nextAttackTime)
                {
                    // Keressük az IDamageable interfészt a célponton (a PlayerCapsule-on)
                    IDamageable playerDamageable = target.GetComponent<IDamageable>();
                    
                    if (playerDamageable != null)
                    {
                        playerDamageable.TakeDamage(damage);
                        Debug.Log("Zombi TÁMAD! Játékos kapott sebzést: " + damage);
                    }
                    
                    nextAttackTime = Time.time + 1f / attackRate; // Beállítja a következő támadás időpontját
                }
            }
        }
    }

    // Az IDamageable interfész megvalósítása
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " sebződött. Új HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Halál
    void Die()
    {
        Debug.Log(gameObject.name + " elpusztult!");
        
        // Jelentjük a halálunkat a WaveManager-nek:
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }
        
        // Megsemmisítjük a zombi objektumot
        Destroy(gameObject);
    }

    // NavMeshAgent és Animator koordinálása
    private void OnAnimatorMove()
{
    // Ezt a metódust csak akkor hívja meg a Unity, ha az Apply Root Motion be van kapcsolva!
    
    // HIBA JAVÍTÁSA: Ellenőrizzük, hogy az 'agent' (NavMeshAgent) komponens létezik-e,
    // különben NullReferenceException-t kapunk, ha a Start() még nem futott le.
    if (agent != null && agent.enabled)
    {
        // Kényszerítjük az objektum pozícióját a NavMeshAgent-ére,
        // felülírva az Animátor által okozott eltolódást.
        transform.position = agent.nextPosition;
    }
}

    // Vizuális segítség a látótávolsághoz (Gizmo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}