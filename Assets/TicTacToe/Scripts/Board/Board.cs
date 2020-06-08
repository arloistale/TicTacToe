using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a tic tac toe board.
/// 
/// The board consists of pieces and lines, where pieces fall on cells on the grid,
/// and lines are drawn to separate the cells
/// 
/// The pieces are intended to be placed in alternating color and shape based on
/// the current player. The naming convention "red" and "black" is used to identify
/// the two players in code, with red player going first and black player going second.
/// 
/// This class uses two conventions to refer to locations on the board.
/// "Point" refers to exact world positions on the board.
/// "Coords" or coordinates refers to discrete rows and columns on the board.
/// 
/// This class provides functions to translate between these two conventions.
/// </summary>
public class Board : MonoBehaviour
{
    private const int COORDS_WIDTH = 3;
    private const int COORDS_HEIGHT = 3;

    private const float CELL_SIZE = 1.75f;

    [SerializeField]
    private Line linePrefab;

    [SerializeField]
    private Piece redPiecePrefab;

    [SerializeField]
    private Piece blackPiecePrefab;

    [SerializeField]
    private Transform pieceHolder;

    [SerializeField]
    private Transform lineHolder;

    // the logical bounds of the board, which represents the total area the player can interact with
    private Vector2 boundsStartPoint;
    private Vector2 boundsSize;

    private List<Line> lines;
    private Piece[] pieces;

    private void Awake()
    {
        CalculateBoardGeometry();
        Init();
    }

    private void CalculateBoardGeometry()
    {
        boundsSize = new Vector2(CELL_SIZE * COORDS_WIDTH, CELL_SIZE * COORDS_HEIGHT);
        boundsStartPoint = new Vector2(transform.position.x - boundsSize.x / 2f, transform.position.y - boundsSize.y / 2f);
    }

    private void Init()
    {
        pieces = new Piece[COORDS_WIDTH * COORDS_HEIGHT];
        lines = new List<Line>();
    }

    /// <summary>
    /// This function should be called at the start of the game to draw the
    /// lines visualizing the board.
    /// </summary>
    public void SpawnLines()
    {
        Vector2 horizontalLinesStart = new Vector2(transform.position.x, boundsStartPoint.y + CELL_SIZE);

        // draw n - 1 lines in horizontal and vertical directions

        for (int r = 0; r < COORDS_HEIGHT - 1; r++)
        {
            Vector2 lineOrigin = horizontalLinesStart + new Vector2(0f, CELL_SIZE * r);
            var line = Instantiate(linePrefab, lineOrigin, Quaternion.identity);
            line.transform.SetParent(lineHolder);
            line.ScaleHorizontally(CELL_SIZE * COORDS_WIDTH);
            lines.Add(line);
        }

        Vector2 verticalLinesStart = new Vector2(boundsStartPoint.x + CELL_SIZE, transform.position.y);

        for (int c = 0; c < COORDS_WIDTH - 1; c++)
        {
            Vector2 lineOrigin = verticalLinesStart + new Vector2(CELL_SIZE * c, 0f);
            var line = Instantiate(linePrefab, lineOrigin, Quaternion.identity);
            line.transform.SetParent(lineHolder);
            line.ScaleVertically(CELL_SIZE * COORDS_HEIGHT);
            lines.Add(line);
        }
    }

    /// <summary>
    /// Resets the board by clearing all pieces and lines.
    /// </summary>
    public void Clear()
    {
        for (int r = 0; r < COORDS_HEIGHT; r++)
        {
            for (int c = 0; c < COORDS_WIDTH; c++)
            {
                var piece = GetPiece(c, r);

                if (piece == null)
                {
                    continue;
                }

                Destroy(piece.gameObject);
            }
        }

        pieces = new Piece[COORDS_WIDTH * COORDS_HEIGHT];

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();
    }

    /// <summary>
    /// Puts a piece down at the given coordinates and with the given color.
    /// </summary>
    /// <param name="coords">The coordinates.</param>
    /// <param name="isRed">Whether it is the red or black player.</param>
    public void PlacePiece(Vector2Int coords, bool isRed)
    {
        Vector2 piecePosition = GetPointAtCoords(coords);
        var piecePrefab = isRed ? redPiecePrefab : blackPiecePrefab;
        var piece = Instantiate(piecePrefab);
        piece.transform.SetParent(pieceHolder);
        piece.transform.localPosition = piecePosition;
        piece.Place();

        pieces[coords.y * COORDS_WIDTH + coords.x] = piece;
    }

    public Vector2 GetPointAtCoords(Vector2Int coords)
    {
        Vector2 deltaPosition = new Vector2(coords.x * CELL_SIZE, coords.y * CELL_SIZE);

        return boundsStartPoint + Vector2.one * CELL_SIZE / 2f + deltaPosition;
    }

    public Vector2Int GetCoordsAtPoint(Vector2 point)
    {
        Vector2 deltaPosition = point - boundsStartPoint;

        int coordsX = (int)(deltaPosition.x / CELL_SIZE);
        int coordsY = (int)(deltaPosition.y / CELL_SIZE);

        return new Vector2Int(coordsX, coordsY);
    }

    /// <summary>
    /// Check whether the given point falls within the world geometry bounds of the board.
    /// </summary>
    /// <returns><c>true</c>, if point is within bounds, <c>false</c> otherwise.</returns>
    /// <param name="point">The given point.</param>
    public bool IsPointWithinBounds(Vector2 point)
    {
        Rect geometryRect = new Rect(boundsStartPoint, boundsSize);

        return geometryRect.Contains(point);
    }

    /// <summary>
    /// Check the state of the game from the perspective of the board,
    /// given the player we are checking for.
    /// 
    /// This check should be made after every time a player makes a move.
    /// 
    /// The game can be either still playing, or one of the two players has won,
    /// or there is a draw.
    /// </summary>
    /// <param name="isRedPlayer">Whether it was the red or black player that made a move.</param>
    /// <param name="moveCoords">The coordinates where they made the move</param>
    public GameState CheckGameStateAfterMove(bool isRedPlayer, Vector2Int moveCoords)
    {
        // check horizontal
        for (int c = 0; c < COORDS_WIDTH; c++)
        {
            var piece = GetPiece(c, moveCoords.y);

            if (piece == null || piece.IsRed != isRedPlayer)
            {
                break;
            }

            if (c == COORDS_WIDTH - 1)
            {
                return GameState.PlayerWon;
            }
        }

        // check vertical
        for (int r = 0; r < COORDS_WIDTH; r++)
        {
            var piece = GetPiece(moveCoords.x, r);

            if (piece == null || piece.IsRed != isRedPlayer)
            {
                break;
            }

            if (r == COORDS_HEIGHT - 1)
            {
                return GameState.PlayerWon;
            }
        }

        // check bottom-left -> top-right diagonal
        // TODO: this method of checking diagonals won't work for non-square boards
        for (int i = 0; i < COORDS_WIDTH; i++)
        {
            var piece = GetPiece(i, i);

            if (piece == null || piece.IsRed != isRedPlayer)
            {
                break;
            }

            if (i == COORDS_WIDTH - 1)
            {
                return GameState.PlayerWon;
            }
        }

        // check top-left -> bottom-right diagonal
        for (int i = 0; i < COORDS_WIDTH; i++)
        {
            var piece = GetPiece(i, COORDS_WIDTH - 1 - i);

            if (piece == null || piece.IsRed != isRedPlayer)
            {
                break;
            }

            if (i == COORDS_WIDTH - 1)
            {
                return GameState.PlayerWon;
            }
        }

        int numEmptySquares = 0;

        // if neither player has won
        // count empty squares to determine if the game is drawn or we are still playing
        for (int r = 0; r < COORDS_HEIGHT; r++)
        {
            for (int c = 0; c < COORDS_WIDTH; c++)
            {
                var piece = GetPiece(c, r);

                if (piece == null)
                {
                    numEmptySquares++;
                }
            }
        }

        if (numEmptySquares == 0)
        {
            return GameState.Draw;
        }

        return GameState.Playing;
    }

    public Piece GetPiece(int col, int row)
    {
        return pieces[row * COORDS_WIDTH + col];
    }

    public Piece GetPiece(Vector2Int coords)
    {
        return GetPiece(coords.x, coords.y);
    }
}
