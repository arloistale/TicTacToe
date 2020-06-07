using UnityEngine;

/// <summary>
/// Represents a tic tac toe board.
/// 
/// The board consists of tiles and pieces, where tiles make up the grid slots
/// in the board, and the pieces are placed onto the tiles.
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

    private const float TILE_SIZE = 1.125f;

    [SerializeField]
    private Tile tilePrefab;

    [SerializeField]
    private Piece redPiecePrefab;

    [SerializeField]
    private Piece blackPiecePrefab;

    [SerializeField]
    private Transform pieceHolder;

    [SerializeField]
    private Transform tileHolder;

    private Vector2 geometryStartPosition;
    private Vector2 geometrySize;

    private Tile[] tiles;
    private Piece[] pieces;

    private void Awake()
    {
        CalculateBoardGeometry();
        InitBoard();
    }

    private void CalculateBoardGeometry()
    {
        geometrySize = new Vector2(TILE_SIZE * COORDS_WIDTH, TILE_SIZE * COORDS_HEIGHT);
        geometryStartPosition = new Vector2(transform.position.x - geometrySize.x / 2f, transform.position.y - geometrySize.y / 2f);
    }

    private void InitBoard()
    {
        tiles = new Tile[COORDS_WIDTH * COORDS_HEIGHT];
        pieces = new Piece[COORDS_WIDTH * COORDS_HEIGHT];

        for (int r = 0; r < COORDS_HEIGHT; r++)
        {
            for (int c = 0; c < COORDS_WIDTH; c++)
            {
                Vector2 tilePosition = new Vector2(geometryStartPosition.x + TILE_SIZE / 2f + TILE_SIZE * c, geometryStartPosition.y + TILE_SIZE / 2f + TILE_SIZE * r);
                var tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.SetParent(tileHolder);

                tiles[r * COORDS_WIDTH + c] = tile;
            }
        }
    }

    public void ClearPieces()
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
    }

    public void PlacePiece(Vector2Int coords, bool isRed)
    {
        Vector2 piecePosition = GetPointAtCoords(coords);
        var piecePrefab = isRed ? redPiecePrefab : blackPiecePrefab;
        var piece = Instantiate(piecePrefab);
        piece.transform.SetParent(pieceHolder);
        piece.transform.localPosition = piecePosition;

        pieces[coords.y * COORDS_WIDTH + coords.x] = piece;
    }

    public Vector2 GetPointAtCoords(Vector2Int coords)
    {
        Vector2 deltaPosition = new Vector2(coords.x * TILE_SIZE, coords.y * TILE_SIZE);

        return geometryStartPosition + Vector2.one * TILE_SIZE / 2f + deltaPosition;
    }

    public Vector2Int GetCoordsAtPoint(Vector2 point)
    {
        Vector2 deltaPosition = point - geometryStartPosition;

        int coordsX = (int)(deltaPosition.x / TILE_SIZE);
        int coordsY = (int)(deltaPosition.y / TILE_SIZE);

        return new Vector2Int(coordsX, coordsY);
    }

    /// <summary>
    /// Check whether the given point falls within the world geometry bounds of the board.
    /// </summary>
    /// <returns><c>true</c>, if point is within bounds, <c>false</c> otherwise.</returns>
    /// <param name="point">The given point.</param>
    public bool IsPointWithinBounds(Vector2 point)
    {
        Rect geometryRect = new Rect(geometryStartPosition, geometrySize);

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
    public GameState GetGameStateAfterMove(bool isRedPlayer, Vector2Int moveCoords)
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
