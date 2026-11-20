using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The main menu builds its level buttons from what was saved: locked until SaveData says
// the level is open, with the best time if there is one.
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] buttons;   // Level 1 to 5, in order

    public GameObject Newest { get; private set; }  // the furthest level open, selected as the menu opens

    void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int level = i + 1;   // build index n is Level n, so one number is the button, the scene and the key
            Button button = buttons[i];
            bool open = SaveData.IsUnlocked(level);

            button.interactable = open;
            button.transform.Find("Title").GetComponent<TMP_Text>().text = "Level " + level;
            button.transform.Find("Info").GetComponent<TMP_Text>().text =
                !open ? "Locked"
                : SaveData.HasBest(level) ? $"Best {SaveData.Best(level):0.0} s"
                : "New";

            // a lambda per button: level is declared inside the loop, so each keeps its own
            button.onClick.AddListener(() => LevelLoader.Instance.Load(level));

            if (open)
                Newest = button.gameObject;
        }
    }
}
