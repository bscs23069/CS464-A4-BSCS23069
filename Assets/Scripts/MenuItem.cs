using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// On every button and slider. The mouse and the keys share one highlight: pointing at an item
// selects it, and the selected item shows a gold ring. Buttons click through the SfxPlayer.
public class MenuItem : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject ring;
    [SerializeField] AudioClip click;

    Selectable item;

    void Awake()
    {
        item = GetComponent<Selectable>();
        ring.SetActive(false);
        if (item is Button button)
            button.onClick.AddListener(() => SfxPlayer.Instance.Play(click));
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (item.IsInteractable())
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData e) => ring.SetActive(true);
    public void OnDeselect(BaseEventData e) => ring.SetActive(false);

    // a panel that closes while this is selected mustn't leave the ring on for next time
    void OnDisable() => ring.SetActive(false);
}
