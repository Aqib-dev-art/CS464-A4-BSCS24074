using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Every menu panel selects its first button as it opens, so the arrow keys and Enter work at once.
public class MenuPanel : MonoBehaviour
{
    [SerializeField] Button firstButton;

    void OnEnable() => Select();
    void Start() => Select();     // the EventSystem may not exist yet when the scene starts

    public void Select()
    {
        if (firstButton == null || EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }
}
