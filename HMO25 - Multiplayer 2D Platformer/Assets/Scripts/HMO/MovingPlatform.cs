using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    private float startingPosX;
    public float maxDistance = 3f;
    void Start()
    {
        startingPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
