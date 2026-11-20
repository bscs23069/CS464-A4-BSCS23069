using UnityEngine;

// Listener 1: the bar on the HUD. The HUD is a prefab, so it finds the player's Health itself.
[RequireComponent(typeof(HealthBar))]
public class HealthBarListener : MonoBehaviour
{
    [SerializeField] Health health;
    HealthBar bar;

    void Awake()
    {
        bar = GetComponent<HealthBar>();
        if (health == null)
            health = FindAnyObjectByType<Health>();
    }

    void OnEnable()
    {
        health.Damaged += Show;
        health.Restored += Show;
    }

    void OnDisable()
    {
        health.Damaged -= Show;
        health.Restored -= Show;
    }

    // a full bar to start with: Health's Awake has run by now
    void Start() => Show(health.Current, health.Max);

    void Show(int current, int max) => bar.Set(current, max);
}
