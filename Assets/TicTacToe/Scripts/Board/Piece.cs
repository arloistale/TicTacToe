using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    private const float TWEEN_DURATION = 0.35f;

    [SerializeField]
    private bool isRed;

    [SerializeField]
    private AudioSource placeAudio;

    public bool IsRed => isRed;

    public void Place()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, TWEEN_DURATION)
            .SetEase(Ease.OutBounce);

        placeAudio.Play();
    }
}
