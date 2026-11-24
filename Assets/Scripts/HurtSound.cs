using UnityEngine;

// Listener 2: the hit sound, played on the SfxPlayer.
[RequireComponent(typeof(Health))]
public class HurtSound : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    Health health;

    void Awake() => health = GetComponent<Health>();
    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max) => SfxPlayer.Instance.Play(clip);
}
