using UnityEngine;
using UnityEngine.AI; 

public class ZombieAI : MonoBehaviour, IDamageable
{
    // Publikus változók (Inspectorban állítható)
    public float maxHealth = 50f;
    public float currentHealth;
    public float chaseDistance = 15f; 
    public float lookRadius = 10f; 
    public float damage = 10f; // A zombi ennyi sebzést okoz.
    public float attackRate = 1f; // Mennyi időnként támad (pl. 1mp)

    // Privát hivatkozások
    private Transform target; 
    private NavMeshAgent agent; 
    private Animator anim;
    private WaveManager waveManager; 
    private PlayerHealth playerHealth; // Hivatkozás a PlayerHealth scriptre
    private AudioSource alertSoundSource; // Riadó/Morajlás hang
    private AudioSource deathSoundSource; // Halál hang
    
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

        // AudioSource komponensek keresése
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 1)
        {
            alertSoundSource = audioSources[0]; // Első AudioSource = Riadó hang
        }
        if (audioSources.Length >= 2)
        {
            deathSoundSource = audioSources[1]; // Második AudioSource = Halál hang
        }

        // Riadó Hang indítása (mivel loop-ra van állítva, folyamatosan szólni fog)
        if (alertSoundSource != null)
        {
            alertSoundSource.Play(); // <<< EZ A SOR INDÍTJA A MORAJLÁST
        }

        waveManager = FindFirstObjectByType<WaveManager>();
    }

    // Minden frame-ben lefut
    void Update()
    {
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
                        // 1. LOGOLJUK A TÁMADÁST
                        Debug.Log("Zombi TÁMAD! Játékos kapott sebzést: " + damage);
                        
                        // 2. OKOZZUK A SEBZÉST (Ez hívja meg a PlayerHealth.Die()-t, ami logolja a GAME OVER-t)
                        playerDamageable.TakeDamage(damage); 
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

    // Halál
    void Die()
    {
        Debug.Log(gameObject.name + " elpusztult!");
        
        // Riadó hang leállítása
        if (alertSoundSource != null)
        {
            alertSoundSource.Stop();
        }

        // Halál hang lejátszása
        if (deathSoundSource != null && deathSoundSource.clip != null)
        {
            deathSoundSource.Play();
            
            // Letiltjuk a mozgást és az ütközést, hogy a zombi azonnal "halott" legyen
            agent.enabled = false;
            GetComponent<Collider>().enabled = false; 
            
            // Elpusztítás késleltetése a hang hossza után
            Destroy(gameObject, deathSoundSource.clip.length); // <<< KÉSLELTETETT PUSZTÍTÁS
        } 
        else 
        {
            // Ha nincs halál hang, azonnal pusztítjuk
            Destroy(gameObject);
        }
        
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