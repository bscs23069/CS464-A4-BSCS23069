using UnityEngine;

// Listener 3: one burst of sparks on the player, no loop.
[RequireComponent(typeof(Health))]
public class HitSparks : MonoBehaviour
{
    [SerializeField] ParticleSystem sparks;
    Health health;

    void Awake() => health = GetComponent<Health>();
    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max) => sparks.Play();
}
