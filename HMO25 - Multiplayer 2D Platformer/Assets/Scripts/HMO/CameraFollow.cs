using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerToFollow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerToFollow.transform.position.x, transform.position.y, transform.position.z);
    }
}
