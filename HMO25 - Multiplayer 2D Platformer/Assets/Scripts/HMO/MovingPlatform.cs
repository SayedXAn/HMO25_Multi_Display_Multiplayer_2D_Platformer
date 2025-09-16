using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    private float startingPosX;
    public float maxDistance = 3f;
    void Start()
    {
        startingPosX = transform.position.x;
        DOTween.Init();
        InvokeRepeating("MoveTheTile", 0.5f, 4f);
    }

    private void MoveTheTile()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(startingPosX + maxDistance, 2f));
        sequence.Append(transform.DOMoveX(startingPosX - maxDistance, 2f));
    }
}
