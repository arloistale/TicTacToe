using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI presenter class that shows end of game visuals.
/// </summary>
public class GameEndedPresenter : MonoBehaviour
{
    private static readonly Color32 redPlayerColor = new Color32(255, 0, 0, 255);
    private static readonly Color32 blackPlayerColor = new Color32(55, 55, 55, 255);
    private static readonly Color32 drawColor = new Color32(255, 255, 255, 255);

    private static readonly Color32 playerWonTextColor = new Color32(255, 255, 255, 255);
    private static readonly Color32 drawTextColor = new Color32(55, 55, 55, 255);

    [SerializeField]
    private GameObject holderObject;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Text backgroundText;

    public void ClearVisuals()
    {
        holderObject.SetActive(false);
    }

    public void PresentGameEndedVisuals(GameState gameState, bool isRedPlayerWinner)
    {
        holderObject.SetActive(true);

        if (gameState == GameState.Draw)
        {
            backgroundImage.color = drawColor;
            backgroundText.color = drawTextColor;
            backgroundText.text = "Draw!\nTap to restart";
            
            return;
        }

        // somebody won
        backgroundText.color = playerWonTextColor;
        backgroundText.text = " wins!\nTap to restart";

        if (isRedPlayerWinner)
        {
            backgroundImage.color = redPlayerColor;
        }
        else
        {
            backgroundImage.color = blackPlayerColor;
        }
    }
}
