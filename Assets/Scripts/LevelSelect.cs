using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A button per level, built from SaveData: locked until unlocked, with its best time if it has one.
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button template;       // inactive; copied once per level
    [SerializeField] Transform container;

    LevelLoader loader;

    void Start()
    {
        Button first = null;
        for (int level = SaveData.FirstLevel; level <= SaveData.LastLevel; level++)
        {
            int n = level;                      // capture
            Button b = Instantiate(template, container);
            b.gameObject.SetActive(true);
            b.name = "Level" + n;

            bool open = SaveData.IsUnlocked(n);
            b.interactable = open;
            if (open && first == null) first = b;
            TMP_Text label = b.GetComponentInChildren<TMP_Text>();
            if (open)
            {
                string best = SaveData.HasBest(n) ? "   best " + SaveData.FormatTime(SaveData.BestTime(n)) : "";
                label.text = "Level " + n + best;
                b.onClick.AddListener(() => Loader.Load(n));
            }
            else
            {
                label.text = "Level " + n + "   locked";
                label.color = Palette.Light;    // on the dark disabled colour
            }
        }
        // The buttons only exist now, so select the first open one here.
        if (first != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }

    LevelLoader Loader => loader != null ? loader : (loader = LevelLoader.Find());
}
