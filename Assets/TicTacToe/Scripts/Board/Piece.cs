using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    private const float PLACE_TWEEN_DURATION = 0.35f;

    [SerializeField]
    private bool isRed;

    [SerializeField]
    private AudioClip placeSound;

    public bool IsRed => isRed;

    public void Place()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, PLACE_TWEEN_DURATION)
            .SetEase(Ease.OutBounce);

        AudioSource.PlayClipAtPoint(placeSound, transform.position, 1f);
    }
}
