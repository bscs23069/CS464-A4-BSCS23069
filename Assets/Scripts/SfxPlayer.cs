using UnityEngine;

// Every sound effect goes through here: SfxPlayer.Instance.Play(clip).
// It survives loads, so the click that restarts a level isn't cut off by the restart,
// and a coin's sound isn't destroyed with the coin.
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [SerializeField] AudioSource[] sources;   // four, each with Output on the SFX group
    [SerializeField] float semitones = 2f;    // random pitch, up to 2 semitones either way
    [SerializeField] float volumeJitter = 0.1f;

    int next;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // the menu clicks still play while AudioListener.pause holds the world sounds
        foreach (AudioSource s in sources)
            s.ignoreListenerPause = true;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        // pitch belongs to the source, so take turns: changing it can't bend a sound still playing
        AudioSource source = sources[next];
        next = (next + 1) % sources.Length;

        source.pitch = Mathf.Pow(2f, Random.Range(-semitones, semitones) / 12f);
        source.PlayOneShot(clip, volume * Random.Range(1f - volumeJitter, 1f));
    }
}
