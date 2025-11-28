using UnityEngine;
using UnityEngine.AI; // NavMeshAgent-hez

public class ZombieAI : MonoBehaviour, IDamageable
{
    // Publikus változók (Inspectorban állítható)
    public float maxHealth = 50f;
    public float currentHealth;
    public float chaseDistance = 15f; // Ezen belül kezdi el a követést
    public float lookRadius = 10f; // A látótávolság sugara

    // Privát hivatkozások
    private Transform target; // A játékos pozíciója
    private NavMeshAgent agent; // A Nav Mesh Agent komponens
    
    // Kezdeti beállítások
    void Start()
    {
        currentHealth = maxHealth;
        // Játékos megkeresése (Player Tag alapján!)
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; 
    agent = GetComponent<NavMeshAgent>();
    }

    // Minden frame-ben lefut
    void Update()
    {
        // Távolság kiszámítása a játékos és a zombi között
        float distance = Vector3.Distance(target.position, transform.position);

        // Ha a játékos túl közel van (chaseDistance)
        if (distance <= chaseDistance)
        {
            // Követés
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Itt lenne a támadás logikája (később)
                //Debug.Log("Zombi Támad!");
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
        // Megsemmisítjük a zombi objektumot
        Destroy(gameObject);
    }

    // Vizuális segítség a látótávolsághoz
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void OnAnimatorMove()
{
    // Ezt a metódust csak akkor hívja meg a Unity, ha az Apply Root Motion be van kapcsolva!
    if (agent.enabled)
    {
        // Kényszerítjük az objektum pozícióját a NavMeshAgent-ére.
        // Ezzel felülírjuk az Animátor által okozott eltolódást.
        transform.position = agent.nextPosition;
    }
}
}