using UnityEngine;
using UnityEngine.UI;

// Two Images filled from the left: the fill, and a paler lag bar behind it that waits 0.4 s, then drains 0.8 per s.
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image lag;
    [SerializeField] float lagDelay = 0.4f;
    [SerializeField] float lagSpeed = 0.8f;

    Health health;
    float target = 1f, lagValue = 1f, holdUntil;

    void OnEnable()
    {
        health = FindAnyObjectByType<Health>();
        if (health == null) return;
        health.Damaged += OnHealthChanged;
        health.Healed += OnHealthChanged;
    }

    void OnDisable()
    {
        if (health == null) return;
        health.Damaged -= OnHealthChanged;
        health.Healed -= OnHealthChanged;
        health = null;
    }

    void Start()
    {
        if (health != null) { Set(health.Current, health.Max); lagValue = target; }
    }

    void OnHealthChanged(int current, int max) => Set(current, max);

    public void Set(int current, int max)
    {
        target = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
        if (fill) fill.fillAmount = target;
        if (target >= lagValue) lagValue = target;               // healing: no lag
        else holdUntil = Time.unscaledTime + lagDelay;
    }

    void Update()
    {
        if (lagValue > target && Time.unscaledTime >= holdUntil)
            lagValue = Mathf.MoveTowards(lagValue, target, lagSpeed * Time.unscaledDeltaTime);
        if (lag) lag.fillAmount = lagValue;
    }
}
