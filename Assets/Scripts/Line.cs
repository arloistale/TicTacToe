using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour
{
    private const float LINE_WIDTH = 0.15f;
    private const float TWEEN_DURATION = 0.5f;

    public void ScaleHorizontally(float length)
    {
        transform.localScale = new Vector3(0f, LINE_WIDTH, 1f);
        transform.DOScale(new Vector3(length, LINE_WIDTH, 1f), TWEEN_DURATION);
    }

    public void ScaleVertically(float length)
    {
        transform.localScale = new Vector3(LINE_WIDTH, 0f, 1f);
        transform.DOScale(new Vector3(LINE_WIDTH, length, 1f), TWEEN_DURATION);
    }
}
