using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    public Slider slider;

    public float maxHealth = 100f;
    public float currentHealth;
    public float chaseDistance = 15f; 
    public float lookRadius = 10f; 
    public float minDamage = 5f;
    public float maxDamage = 15f;
    public float attackRate = 1f; 
    
    public AudioSource alertSoundSource; 
    public AudioSource deathSoundSource; 

    private Transform target; 
    private NavMeshAgent agent; 
    private Animator anim;
    private WaveManager waveManager; 
    private PlayerHealth playerHealth; 
    
    private float nextAttackTime = 0f;

    void Start()
    {
        alertSoundSource.volume = Settings.gameVolume / 100f;
        deathSoundSource.volume = Settings.gameVolume / 100f;
        
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.minValue = 0;

        slider.value = currentHealth;
        
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (target != null)
        {
            playerHealth = target.GetComponent<PlayerHealth>();
        }
        
        waveManager = FindFirstObjectByType<WaveManager>();

        if (alertSoundSource != null)
        {
            alertSoundSource.Play();
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return; 

        float distance = Vector3.Distance(target.position, transform.position);

        float speed = agent.velocity.magnitude / agent.speed; 
        if (anim != null)
        {
            anim.SetFloat("Speed", speed); 
        }

        if (distance <= chaseDistance)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                if (playerHealth != null && playerHealth.GetCurrentHealth() > 0 && Time.time >= nextAttackTime)
                {
                    IDamageable playerDamageable = target.GetComponent<IDamageable>();
                    
                    if (playerDamageable != null)
                    {
                        int damage = (int)Random.Range(minDamage, maxDamage);
                        Debug.Log("Zombi TÁMAD! Játékos kapott sebzést: " + damage);
                        playerDamageable.TakeDamage(damage); 
                    }
                    
                    if(anim != null)
                    {
                        anim.SetTrigger("Attack");
                    }
                    
                    nextAttackTime = Time.time + 1f / attackRate; 
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        slider.value = currentHealth;
        Debug.Log(gameObject.name + " sebződött. Új HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            slider.enabled = false;
            slider.gameObject.SetActive(false);
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " elpusztult!");
        
        agent.enabled = false;
        GetComponent<Collider>().enabled = false; 
        
        if (anim != null)
        {
            anim.SetTrigger("IsDead"); 
            Debug.Log("Animáció: IsDead Trigger beállítva."); 
        }
        else
        {
            Debug.LogError("Hiba: Az Animator referencia hiányzik a zombiról!");
        }
        
        if (alertSoundSource != null)
        {
            alertSoundSource.Stop();
        }

        if (deathSoundSource != null)
        {
            deathSoundSource.Play();
        } 
        
        const float FIXED_DEATH_DELAY = 3.25f; 
        Destroy(gameObject, FIXED_DEATH_DELAY); 
        
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }
    }

    private void OnAnimatorMove()
    {
        if (agent != null && agent.enabled)
        {
            transform.position = anim.rootPosition; 
            transform.rotation = anim.rootRotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}