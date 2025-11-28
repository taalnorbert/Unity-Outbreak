using UnityEngine;
using UnityEngine.AI; 

public class ZombieAI : MonoBehaviour, IDamageable
{
    // Publikus változók (Inspectorban állítható)
    public float maxHealth = 50f;
    public float currentHealth;
    public float chaseDistance = 15f; 
    public float lookRadius = 10f; 
    public float damage = 10f; 
    public float attackRate = 1f; 
    
    // Hang slotok
    public AudioSource alertSoundSource; // Riadó/Morajlás hang slot
    public AudioSource deathSoundSource; // Halál hang slot

    // Privát hivatkozások
    private Transform target; 
    private NavMeshAgent agent; 
    private Animator anim;
    
    // A hiba javítása: EZ A VÁLTOZÓ KELL A CLASS ELEJÉRE!
    private WaveManager waveManager; 
    private PlayerHealth playerHealth; 
    
    private float nextAttackTime = 0f;

    // Kezdeti beállítások
    void Start()
    {
        currentHealth = maxHealth;
        
        // Komponensek és célpontok keresése
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Lekérjük a PlayerHealth komponenst a target-ről
        if (target != null)
        {
            playerHealth = target.GetComponent<PlayerHealth>();
        }
        
        // WaveManager keresése
        waveManager = FindFirstObjectByType<WaveManager>();

        // Riadó Hang indítása
        if (alertSoundSource != null)
        {
            alertSoundSource.Play();
        }
    }

    // Minden frame-ben lefut
    void Update()
    {
        // Ha meghaltunk, ne futtassuk a kódot
        if (currentHealth <= 0) return; 

        // Távolság kiszámítása a játékos és a zombi között
        float distance = Vector3.Distance(target.position, transform.position);

        // Zombi sebesség átadása az Animátornak
        float speed = agent.velocity.magnitude / agent.speed; 
        if (anim != null)
        {
            anim.SetFloat("Speed", speed); 
        }

        // Ha a játékos túl közel van (chaseDistance)
        if (distance <= chaseDistance)
        {
            // Követés
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Támadás Logikája: CSAK AKKOR TÁMADUNK, HA A JÁTÉKOS ÉLETBEN VAN!
                if (playerHealth != null && playerHealth.currentHealth > 0 && Time.time >= nextAttackTime)
                {
                    IDamageable playerDamageable = target.GetComponent<IDamageable>();
                    
                    if (playerDamageable != null)
                    {
                        Debug.Log("Zombi TÁMAD! Játékos kapott sebzést: " + damage);
                        playerDamageable.TakeDamage(damage); 
                    }
                    
                    // Animáció aktiválása itt
                    if(anim != null)
                    {
                        anim.SetTrigger("Attack"); // feltéve, hogy van "Attack" Trigger
                    }
                    
                    nextAttackTime = Time.time + 1f / attackRate; 
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

    // Halál logika (Die)
    void Die()
    {
        Debug.Log(gameObject.name + " elpusztult!");
        
        // 1. Zárjunk ki minden további mozgást és sebzést!
        agent.enabled = false;
        GetComponent<Collider>().enabled = false; 
        
        // Átmenet a 'IsDead' animációra
        if (anim != null)
        {
            // Beállítjuk az 'IsDead' Bool paramétert
            anim.SetBool("IsDead", true); 
        }
        
        // Riadó hang leállítása
        if (alertSoundSource != null)
        {
            alertSoundSource.Stop();
        }

        // 2. Halál hang lejátszása
        if (deathSoundSource != null)
        {
            deathSoundSource.Play();
        } 
        
        // 3. Elpusztítás KÉSLELTETÉSSEL (3.25 másodperc)
        // A kért 3.25 másodperc, hogy az animáció végig fusson.
        const float FIXED_DEATH_DELAY = 3.25f; 
        Destroy(gameObject, FIXED_DEATH_DELAY); 
        
        // 4. Jelzés a WaveManager-nek
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }
    }

    // NavMeshAgent és Animator koordinálása
    private void OnAnimatorMove()
    {
        if (agent != null && agent.enabled)
        {
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