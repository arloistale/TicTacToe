using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI presenter class that shows end of game visuals.
/// </summary>
public class GameEndedPresenter : MonoBehaviour
{
    private static readonly Color32 backgroundColor = new Color32(67, 160, 71, 255);

    [SerializeField]
    private GameObject visualsHolderObject;

    [SerializeField]
    private GameObject winnerVisualsHolderObject;

    [SerializeField]
    private GameObject drawVisualsHolderObject;

    [SerializeField]
    private Image backgroundImage;

    // winner visuals fields

    [SerializeField]
    private Image winnerImage;

    [SerializeField]
    private Sprite redSprite;

    [SerializeField]
    private Sprite blackSprite;

    [SerializeField]
    private AudioSource victoryAudio;

    private void Awake()
    {
        backgroundImage.color = backgroundColor;
    }

    public void Clear()
    {
        visualsHolderObject.SetActive(false);
        victoryAudio.Stop();
    }

    public void PresentVisualsForDraw()
    {
        visualsHolderObject.SetActive(true);

        winnerVisualsHolderObject.SetActive(false);
        drawVisualsHolderObject.SetActive(true);

        TweenVisualsIn(then: () =>
        { 
        });

        victoryAudio.Play();
    }

    public void PresentVisualsForPlayerWon(bool isRedPlayerWinner)
    {
        visualsHolderObject.SetActive(true);

        winnerVisualsHolderObject.SetActive(true);
        drawVisualsHolderObject.SetActive(false);

        if (isRedPlayerWinner)
        {
            winnerImage.sprite = redSprite;
        }
        else
        {
            winnerImage.sprite = blackSprite;
        }

        winnerImage.rectTransform.anchoredPosition = Vector2.zero;
        winnerImage.transform.localScale = Vector3.one;

        TweenVisualsIn(then: () =>
        {
            DOTween.Kill(winnerImage.transform);

            // after the background is in animate the winner's icon
            DOTween.To(() => winnerImage.rectTransform.anchoredPosition.y,
                v =>
                {
                    winnerImage.rectTransform.anchoredPosition = new Vector2(winnerImage.rectTransform.anchoredPosition.x, v);
                }, 60f, 0.5f)
                .SetEase(Ease.InOutBack)
                .SetRelative();

            winnerImage.transform.DOScale(5f, 0.5f)
                .SetEase(Ease.InOutBack);
        });

        victoryAudio.Play();
    }

    private void TweenVisualsIn(System.Action then)
    {
        var rectTransform = visualsHolderObject.transform as RectTransform;
        rectTransform.anchoredPosition = Vector2.zero;
        DOTween.Kill(rectTransform);
        DOTween.To(() => rectTransform.anchoredPosition.y,
            v =>
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, v);
            },
            rectTransform.rect.height, 0.5f)
            .OnComplete(() => then());
    }
}
