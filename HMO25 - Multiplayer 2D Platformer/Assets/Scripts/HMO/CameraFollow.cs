using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerToFollow;
    public float leftThreshold = 1.5f;
    private void Start()
    {
        //gameObject.GetComponent<Camera>().targetDisplay = (int)playerToFollow.GetComponent<PlayerController>().id - 1;
        
    }
    void Update()
    {
        if(playerToFollow != null )
        {
            transform.position = new Vector3(Mathf.Clamp(playerToFollow.transform.position.x + leftThreshold, -8.98f, 46.37f), transform.position.y, transform.position.z);
        }
    }
}
