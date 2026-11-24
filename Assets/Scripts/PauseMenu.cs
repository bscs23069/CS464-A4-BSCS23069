using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// On the HUD's Canvas. Esc pauses and resumes. Every load from a menu calls Resume() first:
// Time.timeScale, AudioListener.pause and IsPaused all survive a scene load.
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstButton;   // Resume

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        Keyboard keys = Keyboard.current;
        if (keys == null || !keys.escapeKey.wasPressedThisFrame)
            return;

        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        // not over an end screen, and not while a level is loading
        if (GameManager.Instance.IsOver || LevelLoader.Instance.IsLoading)
            return;

        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;   // the world sounds stop; the music and the clicks ignore it
        pausePanel.SetActive(true);
        Select(firstButton);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
    }

    public void Restart()
    {
        Resume();
        LevelLoader.Instance.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        LevelLoader.Instance.LoadMenu();
    }

    // the win screen's Next level, which after Level05 is the credits
    public void NextLevel()
    {
        Resume();
        LevelLoader.Instance.LoadNext();
    }

    // First Selected on the EventSystem only covers a menu that is there when the scene starts,
    // so every panel selects its own first button as it opens
    public static void Select(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
