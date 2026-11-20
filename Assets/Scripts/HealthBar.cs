using UnityEngine;
using UnityEngine.UI;

// Two Images, Filled, Horizontal, from the left: the fill in front, a paler lag bar behind
// that keeps showing what a hit just took, then slides down to meet the fill.
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image lag;
    [SerializeField] Gradient colours;      // coral when low, green when healthy
    [SerializeField] float lagDelay = 0.4f; // seconds the lost part stays on show
    [SerializeField] float lagSpeed = 0.8f; // of the whole bar per second

    float lagStartsAt;

    // called when health changes, never polled
    public void Set(int current, int max)
    {
        float fraction = Mathf.Clamp01((float)current / max); // the cast: 2 / 3 as ints is 0

        if (fraction < fill.fillAmount)
            lagStartsAt = Time.unscaledTime + lagDelay;   // a hit: the lag waits, then follows
        else
            lag.fillAmount = Mathf.Max(lag.fillAmount, fraction); // a heal: the lag moves up with it

        fill.fillAmount = fraction;
        fill.color = colours.Evaluate(fraction);
    }

    // unscaled, so the lag still finishes when the hit that ends the game also pauses it
    void Update()
    {
        if (Time.unscaledTime < lagStartsAt)
            return;
        lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, fill.fillAmount, lagSpeed * Time.unscaledDeltaTime);
    }
}
