using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Numbers (the same on the prefab in every level)")]
    [SerializeField] int targetScore = 5;          // coins needed before the goal counts
    [SerializeField] int lives = 3;
    [SerializeField] float respawnDelay = 0.6f;    // a beat before coming back
    [SerializeField] float respawnProtection = 1.5f;

    [Header("Set in each level")]
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;

    [Header("On the HUD prefab")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject messagePanel;
    [SerializeField] TMP_Text messageText;
    [SerializeField] GameObject winPanel;
    [SerializeField] TMP_Text winTimeText;
    [SerializeField] TMP_Text nextLabel;
    [SerializeField] GameObject winFirstButton;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject loseFirstButton;

    [Header("Sounds")]
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;

    int score;
    public bool IsOver { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable() => player.Died += PlayerDied;
    void OnDisable() => player.Died -= PlayerDied;

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        messagePanel.SetActive(false);
        UpdateScore();
        UpdateLives();
    }

    void Update()
    {
        if (!IsOver)
            timeText.SetText("Time {0:1} s", Time.timeSinceLevelLoad);
    }

    public void AddScore(int amount)
    {
        if (IsOver)
            return;

        score += amount;
        UpdateScore();
        if (score == targetScore)
            ShowMessage("The goal is open!");
    }

    public void ReachedGoal()
    {
        if (IsOver)
            return;

        if (score >= targetScore)
            Won();
        else
            ShowMessage($"Collect {targetScore - score} more coins first");
    }

    // build index n is Level n, so one number is the scene, the save key and the next unlock
    public void Won()
    {
        int level = gameObject.scene.buildIndex;
        float time = Time.timeSinceLevelLoad;   // scaled time, so time spent paused doesn't count
        bool best = SaveData.Record(level, time);
        SaveData.Unlock(level + 1);
        ShowWinScreen(level, time, best);
    }

    void ShowWinScreen(int level, float time, bool best)
    {
        IsOver = true;
        Time.timeScale = 0f;
        timeText.SetText("Time {0:1} s", time);
        winTimeText.text = best
            ? $"Time {time:0.0} s  ·  <color=#5EE06B>New best!</color>"
            : $"Time {time:0.0} s  ·  Best {SaveData.Best(level):0.0} s";
        nextLabel.text = level == SaveData.LevelCount ? "Credits" : "Next level";
        winPanel.SetActive(true);
        PauseMenu.Select(winFirstButton);
        SfxPlayer.Instance.Play(winSound);
    }

    void PlayerDied()
    {
        if (IsOver)
            return;

        lives--;
        UpdateLives();

        if (lives <= 0)
            Lose();
        else
            StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        // wait a beat: a knockback queued for the next step must not land on the respawned player
        yield return new WaitForSeconds(respawnDelay);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.position = respawnPoint.position;
        player.transform.position = respawnPoint.position;
        rb.linearVelocity = Vector3.zero;
        player.GetComponent<PlayerController>().ResetState();
        player.Restore(respawnProtection);
    }

    public void SetCheckpoint(Transform point) => respawnPoint = point;

    void Lose()
    {
        IsOver = true;
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        PauseMenu.Select(loseFirstButton);
        SfxPlayer.Instance.Play(loseSound);
    }

    // each text is set only when its number changes
    void UpdateScore() => scoreText.text = $"Coins {score}/{targetScore}";
    void UpdateLives() => livesText.text = $"Lives {Mathf.Max(lives, 0)}";

    void ShowMessage(string text)
    {
        messageText.text = text;
        messagePanel.SetActive(true);
        if (clearing != null)
            StopCoroutine(clearing); // a new message gets its own 2 s
        clearing = StartCoroutine(ClearMessage());
    }

    Coroutine clearing;

    IEnumerator ClearMessage()
    {
        yield return new WaitForSeconds(2f);
        messagePanel.SetActive(false);
    }
}
