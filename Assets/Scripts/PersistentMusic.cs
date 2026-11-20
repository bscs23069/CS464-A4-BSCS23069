using UnityEngine;

// On a root object in every scene, so any scene can be played on its own.
// The first one stays for the whole game; every later copy destroys itself.
[RequireComponent(typeof(AudioSource))]
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Play On Awake is off on the source, so a copy never starts before it is destroyed
        AudioSource source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true; // the music carries on under the pause menu
        source.loop = true;
        source.Play();
    }
}
