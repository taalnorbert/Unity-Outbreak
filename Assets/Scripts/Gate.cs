using UnityEngine;

public class Gate : MonoBehaviour
{
    public void Open()
    {
        Debug.Log(gameObject.name + " kinyílt, Act 2 elérhető.");
        
        gameObject.SetActive(false); 
        
    }
}