using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class HostageCage : MonoBehaviour
{
    public float interactionTimeRequired = 5.0f; 
    public float interactDistance = 2.0f;       
    
    public Transform cageObject; 
    public Vector3 openOffset = new Vector3(0, -5, 0);

    [Header("UI Vizuális Visszajelzés")]
    public GameObject interactionUI;
    public Transform progressBarScaleTarget;

    private float interactionTimer = 0f;
    private bool isRescued = false;

    public UnityEvent OnRescueCompleted; 

    void Start()
    {
        if (cageObject == null) cageObject = transform; 
        
        if (interactionUI != null) interactionUI.SetActive(false); 
        if (progressBarScaleTarget != null) progressBarScaleTarget.localScale = new Vector3(0f, 1f, 1f);
    }

    void Update()
    {
        if (isRescued) return;
        
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (player == null) 
        {
            Debug.LogError("HostageCage: Nem található 'Player' Tag-gel rendelkező objektum!");
            return;
        }
        
        float distance = Vector3.Distance(transform.position, player.position);
        
        bool isCloseEnough = distance <= interactDistance;

        if (interactionUI != null)
        {
            interactionUI.SetActive(isCloseEnough); 
        }

        if (isCloseEnough && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Nyomom az E-t és közel vagyok!");
            
            interactionTimer += Time.deltaTime;
            
            float progress = interactionTimer / interactionTimeRequired;
            
            if (progressBarScaleTarget != null)
            {
                progressBarScaleTarget.localScale = new Vector3(progress, 1f, 1f);
            }
            
            Debug.Log($"Ketrec nyitása: {(progress * 100).ToString("F0")}%");

            if (interactionTimer >= interactionTimeRequired)
            {
                StartRescue();
            }
        }
        else
        {
            if (interactionTimer > 0)
            {
                interactionTimer = 0f;
                if (progressBarScaleTarget != null)
                {
                    progressBarScaleTarget.localScale = new Vector3(0f, 1f, 1f);
                }
                Debug.Log("Interakció megszakítva.");
            }
        }
    }

    void StartRescue()
    {
        isRescued = true;
        Debug.Log("Act 1: Túsz kiszabadítva! Spawn leállítva.");

        StartCoroutine(OpenDoorAnimation());

        if (interactionUI != null) interactionUI.SetActive(false); 
        
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
            cageObject.position = Vector3.Lerp(initialPos, targetPos, t); 
            yield return null;
        }
        GetComponent<Collider>().enabled = false;
    }
}