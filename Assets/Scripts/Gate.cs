using UnityEngine;

public class Gate : MonoBehaviour
{
    // A GameFlowManager ezt a metódust fogja hívni
    public void Open()
    {
        Debug.Log(gameObject.name + " kinyílt, Act 2 elérhető.");
        
        // A kapu eltüntetése a legegyszerűbb mód a nyitásra
        gameObject.SetActive(false); 
        
        // VAGY: Csak a Collider eltávolítása, ha a kapu látható marad:
        // GetComponent<Collider>().enabled = false;
    }
}