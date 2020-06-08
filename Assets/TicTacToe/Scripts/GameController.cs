using UnityEngine;
using System.Collections;

/// <summary>
/// Main controller class for the flow of game logic.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    private Board board;

    [SerializeField]
    private CurrentPlayerPresenter currentPlayerPresenter;

    [SerializeField]
    private GameEndedPresenter gameEndedPresenter;

    private bool isRedTurn;
    private GameState currentState;

    private bool isAcceptingInput;

    private IEnumerator Start()
    {
        currentPlayerPresenter.Clear();
        gameEndedPresenter.Clear();

        yield return new WaitForSeconds(0.25f);

        StartNewGame();
    }

    private void StartNewGame()
    {
        Debug.Log("Started new game.");

        isRedTurn = true;
        currentState = GameState.Playing;

        gameEndedPresenter.Clear();

        currentPlayerPresenter.Init(isRedTurn);

        board.Clear();
        board.SpawnLines();

        isAcceptingInput = true;
    }

    private void EndGame(GameState terminalState)
    {
        Debug.Log("Ended game with state: " + terminalState);
        currentState = terminalState;

        // put a freeze on clicking so the visuals can animate in
        isAcceptingInput = false;

        StartCoroutine(PresentEndOfGameVisuals(terminalState));
    }

    private IEnumerator PresentEndOfGameVisuals(GameState terminalState)
    {
        currentPlayerPresenter.HighlightEndOfGame();

        // wait for a bit
        yield return new WaitForSeconds(1.5f);

        switch (terminalState)
        {
            case GameState.Draw:
                gameEndedPresenter.PresentDrawVisuals();
                break;
            case GameState.PlayerWon:
                gameEndedPresenter.PresentPlayerWonVisuals(isRedTurn);
                break;
        }

        yield return new WaitForSeconds(1.25f);

        isAcceptingInput = true;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || !isAcceptingInput)
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
        Debug.Log("Placing " + (isRedTurn ? "red" : "black") + " piece at " + coords);
        board.PlacePiece(coords, isRedTurn);

        // check board to see if an end condition was reached
        GameState state = board.CheckGameStateAfterMove(isRedTurn, coords);

        if (state == GameState.Playing)
        {
            isRedTurn = !isRedTurn;
            currentPlayerPresenter.HighlightPlayer(isRedTurn);
        }
        else
        {
            EndGame(state);
        }
    }
}
