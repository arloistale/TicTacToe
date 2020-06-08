using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI presenter class that shows end of game visuals.
/// </summary>
public class GameEndedPresenter : MonoBehaviour
{
    private static readonly Color32 redBackgroundColor = new Color32(67, 160, 71, 255);
    private static readonly Color32 blackBackgroundColor = new Color32(67, 160, 71, 255);
    private static readonly Color32 drawBackgroundColor = new Color32(67, 160, 71, 255);

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

    public void ClearVisuals()
    {
        visualsHolderObject.SetActive(false);
    }

    public void PresentGameEndedVisuals(GameState gameState, bool isRedPlayerWinner)
    {
        visualsHolderObject.SetActive(true);

        if (gameState == GameState.Draw)
        {
            backgroundImage.color = drawBackgroundColor;
            winnerVisualsHolderObject.SetActive(false);
            drawVisualsHolderObject.SetActive(true);
            return;
        }

        // somebody won
        winnerVisualsHolderObject.SetActive(true);
        drawVisualsHolderObject.SetActive(false);

        if (isRedPlayerWinner)
        {
            backgroundImage.color = redBackgroundColor;
            winnerImage.sprite = redSprite;
        }
        else
        {
            backgroundImage.color = blackBackgroundColor;
            winnerImage.sprite = blackSprite;
        }
    }
}
