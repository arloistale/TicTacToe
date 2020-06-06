using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerDisplay : MonoBehaviour
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
