using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour
{
    private const float LINE_WIDTH = 0.15f;
    private const float GROW_DURATION = 0.5f;
    private const float RETRACT_DURATION = 0.35f;

    private bool isHorizontal;

    public void ScaleHorizontally(float length)
    {
        isHorizontal = true;
        transform.localScale = new Vector3(0f, LINE_WIDTH, 1f);
        transform.DOScaleX(length, GROW_DURATION);
    }

    public void ScaleVertically(float length)
    {
        isHorizontal = false;
        transform.localScale = new Vector3(LINE_WIDTH, 0f, 1f);
        transform.DOScaleY(length, GROW_DURATION);
    }

    public void Retract()
    {
        if (isHorizontal)
        {
            transform.DOScaleX(0f, RETRACT_DURATION)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
        else
        {
            transform.DOScaleY(0f, RETRACT_DURATION)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}
