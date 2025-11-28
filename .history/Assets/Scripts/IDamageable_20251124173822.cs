// IDamageable.cs
using UnityEngine;

public interface IDamageable
{
    // Ez a metódus határozza meg, hogyan kell sebzést fogadni.
    // A fegyver sebzése (25.0f) float, ezért float-ot használunk itt is.
    void TakeDamage(float damage);
}