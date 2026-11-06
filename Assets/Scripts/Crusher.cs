using System.Collections;
using UnityEngine;

// On the slab, next to Hazard.cs (damage 2). Off -> warning -> strike.
public class Crusher : MonoBehaviour
{
    [SerializeField] float restTime = 2f;
    [SerializeField] float warnTime = 0.7f;    // 0.5 s or more
    [SerializeField] float strikeTime = 0.4f;
    [SerializeField] Renderer slab;
    [SerializeField] Color restColor = Color.gray, warnColor = Color.yellow, strikeColor = Color.red;

    Collider col;

    void Awake() { col = GetComponent<Collider>(); }
    void OnEnable() => StartCoroutine(Cycle());

    IEnumerator Cycle()
    {
        while (true)
        {
            col.enabled = false; slab.material.color = restColor;
            yield return new WaitForSeconds(restTime);
            slab.material.color = warnColor;       // warning: still harmless
            yield return new WaitForSeconds(warnTime);
            col.enabled = true;  slab.material.color = strikeColor;
            yield return new WaitForSeconds(strikeTime);
        }
    }
}
