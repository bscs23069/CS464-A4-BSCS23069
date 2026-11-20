using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 120f; // degrees per second
    [SerializeField] AudioClip sound;
    [SerializeField] GameObject sparkle;     // Stop Action Destroy, so it cleans itself up

    bool collected;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // the built-in Player tag, and tags are case-sensitive
        if (collected || !other.CompareTag("Player"))
            return;

        collected = true;
        GameManager.Instance.AddScore(value);

        // the sound plays on the SfxPlayer and the sparkle is its own object, so neither goes with the coin
        SfxPlayer.Instance.Play(sound);
        Instantiate(sparkle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
