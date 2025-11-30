using UnityEngine;

public class ZombieHPBar : MonoBehaviour
{
    private void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);
    }
}
