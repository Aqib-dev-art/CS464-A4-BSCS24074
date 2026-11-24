using UnityEngine;
using UnityEngine.UI;

// Listener 4: a short colour flash over the screen when the player is hurt.
public class DamageFlash : MonoBehaviour
{
    [SerializeField] Image flash;
    [SerializeField] float peakAlpha = 0.35f;
    [SerializeField] float fadeSeconds = 0.35f;

    Health health;
    float alpha;

    void OnEnable()
    {
        health = FindAnyObjectByType<Health>();
        if (health != null) health.Damaged += OnHit;
    }

    void OnDisable()
    {
        if (health != null) health.Damaged -= OnHit;
        health = null;
    }

    void OnHit(int current, int max) => alpha = peakAlpha;

    void Update()
    {
        if (flash == null) return;
        alpha = Mathf.MoveTowards(alpha, 0f, peakAlpha / fadeSeconds * Time.unscaledDeltaTime);
        Color c = flash.color;
        c.a = alpha;
        flash.color = c;
    }
}
