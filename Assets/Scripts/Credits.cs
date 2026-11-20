using UnityEngine;
using UnityEngine.InputSystem;

// The last scene in play order. Its button, or Esc, goes on through the LevelLoader, which wraps to the menu.
public class Credits : MonoBehaviour
{
    [SerializeField] GameObject backButton;
    [SerializeField] RectTransform roll;      // the text, scrolled up slowly
    [SerializeField] float scrollSpeed = 40f; // px per second
    [SerializeField] float stopAt = 0f;

    void Start() => PauseMenu.Select(backButton);

    void Update()
    {
        if (roll.anchoredPosition.y < stopAt)
            roll.anchoredPosition += Vector2.up * scrollSpeed * Time.unscaledDeltaTime;

        Keyboard keys = Keyboard.current;
        if (keys != null && keys.escapeKey.wasPressedThisFrame)
            Back();
    }

    public void Back() => LevelLoader.Instance.LoadNext();
}
