using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HostageCage : MonoBehaviour
{
    // 5 másodperc lenyomva tartás
    public float interactionTimeRequired = 5.0f; 
    public float interactDistance = 2.0f;       
    
    // Ajtó/Ketrec objektum (ezt fogjuk elmozdítani)
    public Transform cageObject; 
    public Vector3 openOffset = new Vector3(0, -5, 0); // Eltolás a föld alá

    private float interactionTimer = 0f;
    private bool isRescued = false;

    // Ezt hívjuk meg, amikor a rescue megtörtént!
    public UnityEvent OnRescueCompleted; 

    void Start()
    {
        // Győződj meg róla, hogy a CageObject be van húzva
        if (cageObject == null) cageObject = transform; 
    }

    void Update()
    {
        if (isRescued) return;
        
        // Keresse meg a játékost és nézze meg a távolságot
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);

        // Ha a játékos közel van ÉS lenyomva tartja az 'E' gombot
        if (distance <= interactDistance && Input.GetKey(KeyCode.E))
        {
            interactionTimer += Time.deltaTime;
            
            // Itt kellene egy UI progressBar-t frissíteni (később)
            Debug.Log($"Ketrec nyitása: {(interactionTimer / interactionTimeRequired * 100).ToString("F0")}%");

            if (interactionTimer >= interactionTimeRequired)
            {
                StartRescue();
            }
        }
        else
        {
            // Ha felengedi a gombot, a timer nullázódik
            if (interactionTimer > 0)
            {
                interactionTimer = 0f;
                Debug.Log("Interakció megszakítva.");
            }
        }
    }

    void StartRescue()
    {
        isRescued = true;
        Debug.Log("Act 1: Túsz kiszabadítva! Spawn leállítva.");

        // Ajtó eltávolítása (Cube eltolása/eltüntetése)
        StartCoroutine(OpenDoorAnimation());

        // Értesítjük a Flow Managert
        OnRescueCompleted?.Invoke();
    }

    IEnumerator OpenDoorAnimation()
    {
        float t = 0;
        Vector3 initialPos = cageObject.position;
        Vector3 targetPos = initialPos + openOffset;

        while(t < 1f)
        {
            t += Time.deltaTime;
            // A Cube-ot fokozatosan a föld alá toljuk
            cageObject.position = Vector3.Lerp(initialPos, targetPos, t); 
            yield return null;
        }
        // Kikapcsoljuk az ajtó ütközőjét (ha maradt)
        GetComponent<Collider>().enabled = false;
    }
}