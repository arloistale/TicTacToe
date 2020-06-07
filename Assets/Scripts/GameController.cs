using UnityEngine;

/// <summary>
/// Main controller class for the flow of game logic.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    private Board board;

    [SerializeField]
    private CurrentPlayerPresenter currentPlayerDisplay;

    [SerializeField]
    private GameEndedPresenter gameEndedPresenter;

    private bool isRedTurn;
    private GameState currentState;

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        isRedTurn = true;
        currentState = GameState.Playing;

        HighlightCurrentPlayer();
        gameEndedPresenter.ClearVisuals();
        board.ClearPieces();
    }

    private void EndGame(GameState terminalState)
    {
        currentState = terminalState;
        gameEndedPresenter.PresentGameEndedVisuals(terminalState, isRedTurn);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // figure out what to do with the click based on game state
        switch (currentState)
        {
            case GameState.Playing:
                // try to put a piece down if players are still playing
                if (board.IsPointWithinBounds(mouseWorldPosition))
                {
                    Vector2Int coords = board.GetCoordsAtPoint(mouseWorldPosition);

                    // only put down a piece if the square is empty
                    if (board.GetPiece(coords) == null)
                    {
                        MakeCurrentPlayerMove(coords);
                    }
                }

                break;

            case GameState.Draw:
            case GameState.PlayerWon:
                // restart the game
                StartNewGame();
                break;
        }
    }

    private void MakeCurrentPlayerMove(Vector2Int coords)
    {
        PlaceCurrentPlayerPiece(coords);

        GameState state = board.GetGameStateAfterMove(isRedTurn, coords);
        if (state != GameState.Playing)
        {
            EndGame(state);
            return;
        }

        isRedTurn = !isRedTurn;
        HighlightCurrentPlayer();
    }

    private void PlaceCurrentPlayerPiece(Vector2Int coords)
    {
        if (isRedTurn)
        {
            Debug.Log("Placed red piece at " + coords);
            board.PlacePiece(coords, isRed: true);
        }
        else
        {
            Debug.Log("Placed black piece at " + coords);
            board.PlacePiece(coords, isRed: false);
        }
    }

    private void HighlightCurrentPlayer()
    {
        if (isRedTurn)
        {
            currentPlayerDisplay.HighlightRedPlayer();
        }
        else
        {
            currentPlayerDisplay.HighlightBlackPlayer();
        }
    }
}
