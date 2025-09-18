using UnityEngine;
using DG.Tweening;

public class MovingPlatformVertical : MonoBehaviour
{
    private float startingPosY;
    public float maxDistance = 3f;
    void Start()
    {
        startingPosY = transform.position.y;
        DOTween.Init();
        InvokeRepeating("MoveTheTile", 0.5f, 4f);
    }

    private void MoveTheTile()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(startingPosY + maxDistance, 2f));
        sequence.Append(transform.DOMoveY(startingPosY - maxDistance, 2f));
    }
}
