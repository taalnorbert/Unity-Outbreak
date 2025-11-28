using UnityEngine;
using System; // Szükséges a [System.Serializable]-hez

[System.Serializable]
public class Wave
{
    // Publikus változók (a WaveManager Inspectorban állítható)
    public string waveName;
    public GameObject enemyPrefab; // A zombi prefabunk
    public int count; // Hány darab jöjjön
    public float rate; // Milyen gyorsan jöjjenek (pl. 0.5 = 2 zombi/sec)
}