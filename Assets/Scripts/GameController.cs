using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Board board;

    [SerializeField]
    private CurrentPlayerDisplay currentPlayerDisplay;

    private bool isRedTurn;

    private void Awake()
    {
        StartNewGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (board.IsPointWithinBoardBounds(mouseWorldPosition))
            {
                Vector2Int coords = board.GetCoordsFromWorldPoint(mouseWorldPosition);

                if (!board.DoesPieceExistAtCoords(coords))
                {
                    MakeCurrentPlayerMove(coords);
                }
            }
        }
    }

    private void StartNewGame()
    {
        isRedTurn = true;

        HighlightCurrentPlayer();
    }

    private void MakeCurrentPlayerMove(Vector2Int coords)
    {
        PlaceCurrentPlayerPiece(coords);

        isRedTurn = !isRedTurn;

        HighlightCurrentPlayer();
    }

    private void PlaceCurrentPlayerPiece(Vector2Int coords)
    {
        if (isRedTurn)
        {
            Debug.Log("Placed red piece at " + coords);
            board.PlaceRedPiece(coords);
        }
        else
        {
            Debug.Log("Placed black piece at " + coords);
            board.PlaceBlackPiece(coords);
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
