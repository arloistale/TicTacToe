using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI presenter class that highlights the current player's turn.
/// </summary>
public class CurrentPlayerPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject holderObject;

    [SerializeField]
    private Image redPlayerIcon;

    [SerializeField]
    private Image blackPlayerIcon;

    public void Clear()
    {
        holderObject.SetActive(false);
    }

    public void Init(bool isRedPlayerTurn)
    {
        holderObject.SetActive(true);

        redPlayerIcon.gameObject.SetActive(isRedPlayerTurn);
        blackPlayerIcon.gameObject.SetActive(!isRedPlayerTurn);
    }

    public void HighlightRedPlayer()
    {
        redPlayerIcon.gameObject.SetActive(true);
        blackPlayerIcon.gameObject.SetActive(false);
    }

    public void HighlightBlackPlayer()
    {
        redPlayerIcon.gameObject.SetActive(false);
        blackPlayerIcon.gameObject.SetActive(true);
    }
}
