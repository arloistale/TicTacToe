using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI presenter class that highlights the current player's turn.
/// </summary>
public class CurrentPlayerPresenter : MonoBehaviour
{
    [SerializeField]
    private Image redPlayerIcon;

    [SerializeField]
    private Image blackPlayerIcon;

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
