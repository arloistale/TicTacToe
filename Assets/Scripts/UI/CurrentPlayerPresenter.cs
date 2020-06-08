using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI presenter class that highlights the current player's turn.
/// </summary>
public class CurrentPlayerPresenter : MonoBehaviour
{
    private const float HIGHLIGHT_TRANSITION_DURATION = 0.25f;

    [SerializeField]
    private GameObject holderObject;

    [SerializeField]
    private Image redPlayerIcon;

    [SerializeField]
    private Image blackPlayerIcon;

    [SerializeField]
    private Text turnText;

    public void Clear()
    {
        holderObject.SetActive(false);
    }

    public void Init(bool isRedPlayerTurn)
    {
        holderObject.SetActive(true);

        redPlayerIcon.gameObject.SetActive(isRedPlayerTurn);
        blackPlayerIcon.gameObject.SetActive(!isRedPlayerTurn);

        if (isRedPlayerTurn)
        {
            redPlayerIcon.transform.localScale = Vector3.zero;
            redPlayerIcon.transform.DOScale(1f, 0.5f)
                .SetEase(Ease.InOutBack);
        }
        else
        {
            blackPlayerIcon.transform.localScale = Vector3.zero;
            redPlayerIcon.transform.DOScale(1f, 0.5f)
                .SetEase(Ease.InOutBack);
        }

        turnText.transform.localScale = Vector3.zero;
        turnText.transform.transform.DOScale(1f, 0.5f)
            .SetDelay(0.25f)
            .SetEase(Ease.InOutBack);
    }

    public void HighlightPlayer(bool isRed)
    {
        Image activePlayerIcon = isRed ? redPlayerIcon : blackPlayerIcon;
        Image inactivePlayerIcon = isRed ? blackPlayerIcon : redPlayerIcon;

        DOTween.Kill(activePlayerIcon.transform);
        DOTween.Kill(inactivePlayerIcon.transform);

        activePlayerIcon.gameObject.SetActive(true);
        activePlayerIcon.transform.localScale = Vector3.zero;
        activePlayerIcon.transform.DOScale(1f, HIGHLIGHT_TRANSITION_DURATION)
            .SetEase(Ease.InOutBack);

        inactivePlayerIcon.transform.localScale = Vector3.one;
        inactivePlayerIcon.transform.DOScale(0f, HIGHLIGHT_TRANSITION_DURATION)
            .OnComplete(() =>
            {
                inactivePlayerIcon.gameObject.SetActive(false);
            });
    }
}
