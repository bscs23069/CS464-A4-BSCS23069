using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The only script that loads a scene. One in every scene, destroyed and made again with it.
// Scene List: MainMenu 0, Level01-05 at 1-5, Credits 6.
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [SerializeField] int menuIndex = 0;
    [SerializeField] int creditsIndex = 6;   // the last scene in play order: after it, back to the menu
    [SerializeField] GameObject screen;      // the loading screen, hidden until a load starts
    [SerializeField] Image bar;
    [SerializeField] TMP_Text label;
    [SerializeField] float minShow = 0.6f;   // a quick load still shows the bar long enough to read

    public bool IsLoading { get; private set; }

    void Awake()
    {
        Instance = this;
        screen.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // one scene on, and after the credits round to the menu
    public void LoadNext()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        Load(current >= creditsIndex ? menuIndex : current + 1);
    }

    public void Reload() => Load(SceneManager.GetActiveScene().buildIndex);

    public void LoadMenu() => Load(menuIndex);

    public void LoadCredits() => Load(creditsIndex);

    public void Load(int index)
    {
        if (IsLoading)
            return; // a second press while loading does nothing
        IsLoading = true;
        Time.timeScale = 1f; // it survives a load, so set it back here
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(LoadInBackground(index));
    }

    IEnumerator LoadInBackground(int index)
    {
        label.text = "Loading " + Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
        bar.fillAmount = 0f;
        screen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false; // hold at 0.9 until the bar has shown 100%

        float shown = 0f;
        while (op.progress < 0.9f || shown < minShow)
        {
            shown += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            bar.fillAmount = Mathf.Min(progress, shown / minShow);
            yield return null;
        }

        bar.fillAmount = 1f;
        yield return null; // one frame at 100%
        op.allowSceneActivation = true;
    }
}
