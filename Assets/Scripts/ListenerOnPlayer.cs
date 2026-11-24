using UnityEngine;

// My camera follows from 10 m behind and above, so a listener on it would hear a guard beside
// the player as far away. This one stands on the player and faces the way the camera faces,
// so distances are measured from the character and left and right still match the screen.
[RequireComponent(typeof(AudioListener))]
public class ListenerOnPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cam;

    void LateUpdate()
    {
        transform.SetPositionAndRotation(player.position, cam.rotation);
    }
}
