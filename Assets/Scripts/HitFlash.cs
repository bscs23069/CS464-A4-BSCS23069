using UnityEngine;
using UnityEngine.UI;

// Listener 4: the screen edge flashes coral and fades. On the HUD's full-screen Image.
[RequireComponent(typeof(Image))]
public class HitFlash : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] float strength = 0.35f;
    [SerializeField] float fadeTime = 0.35f;

    Image image;
    float alpha;

    void Awake()
    {
        image = GetComponent<Image>();
        if (health == null)
            health = FindAnyObjectByType<Health>();
        SetAlpha(0f);
    }

    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max) => alpha = strength;

    void Update()
    {
        if (alpha <= 0f)
            return;
        alpha = Mathf.MoveTowards(alpha, 0f, strength / fadeTime * Time.unscaledDeltaTime);
        SetAlpha(alpha);
    }

    void SetAlpha(float a)
    {
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}
