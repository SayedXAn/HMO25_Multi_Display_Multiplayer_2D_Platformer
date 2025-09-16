using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerToFollow;
    public float leftThreshold = 1.5f;
    void Update()
    {
        if(playerToFollow != null)
        {
            transform.position = new Vector3(playerToFollow.transform.position.x + leftThreshold, transform.position.y, transform.position.z);
        }
    }
}
