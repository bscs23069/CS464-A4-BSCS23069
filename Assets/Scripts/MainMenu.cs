using UnityEngine;

// On the MainMenu Canvas. The level buttons come from LevelSelect; this selects one for the keys.
public class MainMenu : MonoBehaviour
{
    [SerializeField] LevelSelect levels;

    void Start()
    {
        // the furthest level you can play, so a new unlock is the one already highlighted
        PauseMenu.Select(levels.Newest);
    }

    public void Credits() => LevelLoader.Instance.LoadCredits();

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
