using System;
using UnityEngine;

// Health only says what happened. The bar, the sound, the sparks, the flash and the
// GameManager each listen for themselves, so Health doesn't need to know any of them.
public class Health : MonoBehaviour
{
    [SerializeField] int maxHearts = 3;
    [SerializeField] float iFrames = 1f;   // seconds nothing can hurt you after a hit
    [SerializeField] Renderer body;        // blinks while invulnerable

    public int Current { get; private set; }
    public int Max => maxHearts;

    public event Action<int, int> Damaged;   // a hit landed: current, max
    public event Action<int, int> Restored;  // back to full after a respawn
    public event Action Died;

    float invulnerableUntil;

    void Awake()
    {
        Current = maxHearts;
    }

    void Update()
    {
        if (body != null)
            body.enabled = Time.time >= invulnerableUntil || Mathf.Repeat(Time.time, 0.2f) < 0.1f;
    }

    // a hazard can ask every physics step, so say no while invulnerable or already down
    public bool TakeDamage(int amount)
    {
        if (Current <= 0 || Time.time < invulnerableUntil)
            return false;

        invulnerableUntil = Time.time + iFrames;
        Current = Mathf.Max(Current - amount, 0); // the crusher's 2 can't take the bar below empty
        Damaged?.Invoke(Current, maxHearts);

        if (Current == 0)
            Died?.Invoke();

        return true;
    }

    // a fall: every heart at once
    public void Kill()
    {
        if (Current <= 0)
            return;

        Current = 0;
        Damaged?.Invoke(Current, maxHearts);
        Died?.Invoke();
    }

    public void Restore(float protection)
    {
        Current = maxHearts;
        invulnerableUntil = Time.time + protection;
        Restored?.Invoke(Current, maxHearts);
    }
}
