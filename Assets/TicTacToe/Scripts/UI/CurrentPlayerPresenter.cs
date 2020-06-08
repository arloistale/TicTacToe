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

    [SerializeField]
    private Text gameOverText;

    public void Clear()
    {
        holderObject.SetActive(false);
    }

    public void Init(bool isRedPlayerTurn)
    {
        holderObject.SetActive(true);

        redPlayerIcon.gameObject.SetActive(isRedPlayerTurn);
        blackPlayerIcon.gameObject.SetActive(!isRedPlayerTurn);

        turnText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);

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

    public void HighlightEndOfGame()
    {
        DOTween.Kill(redPlayerIcon.transform);
        DOTween.Kill(blackPlayerIcon.transform);

        var activeIcon = redPlayerIcon.gameObject.activeInHierarchy ? redPlayerIcon : blackPlayerIcon;

        if (redPlayerIcon.gameObject.activeInHierarchy)
        {
            redPlayerIcon.transform.localScale = Vector3.one;
            redPlayerIcon.transform.DOScale(0f, HIGHLIGHT_TRANSITION_DURATION);
        }

        if (blackPlayerIcon.gameObject.activeInHierarchy)
        {
            blackPlayerIcon.transform.localScale = Vector3.one;
            blackPlayerIcon.transform.DOScale(0f, HIGHLIGHT_TRANSITION_DURATION);
        }

        turnText.transform.localScale = Vector3.one;
        turnText.transform.transform.DOScale(0f, 0.25f);

        gameOverText.gameObject.SetActive(true);
        gameOverText.transform.localScale = Vector3.zero;
        gameOverText.transform.transform.DOScale(1f, 0.25f)
            .SetEase(Ease.InOutBack);
    }
}
