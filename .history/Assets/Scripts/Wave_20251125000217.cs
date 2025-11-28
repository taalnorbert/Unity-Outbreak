using UnityEngine;
using System;

[System.Serializable]
public class Wave
{
    public string waveName;
    public GameObject enemyPrefab; 
    public int count; 
    public float rate; // Milyen gyorsan jöjjenek (pl. 0.5 = 2 zombi/sec)
}