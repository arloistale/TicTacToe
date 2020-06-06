using UnityEngine;

public class Board : MonoBehaviour
{
    private const int COORDS_WIDTH = 3;
    private const int COORDS_HEIGHT = 3;

    private const float TILE_SIZE = 1.125f;

    private Vector2 geometryStartPosition;
    private Vector2 geometrySize;

    private Tile[] tiles;
    private Piece[] pieces;

    [SerializeField]
    private Tile tilePrefab;

    [SerializeField]
    private Piece redPiecePrefab;

    [SerializeField]
    private Piece blackPiecePrefab;

    [SerializeField]
    private Transform pieceHolder;

    private void Awake()
    {
        InitGeometry();
        InitBoard();
    }

    private void InitGeometry()
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
                tile.transform.SetParent(transform);

                tiles[r * COORDS_WIDTH + c] = tile;
            }
        }
    }

    public bool DoesPieceExistAtCoords(Vector2Int coords)
    {
        return pieces[coords.y * COORDS_WIDTH + coords.x] != null;
    }

    public void PlaceRedPiece(Vector2Int coords)
    {
        Vector2 piecePosition = GetWorldPointCenteredAtCoords(coords);
        var piece = Instantiate(redPiecePrefab);
        piece.transform.SetParent(pieceHolder);
        piece.transform.localPosition = piecePosition;

        pieces[coords.y * COORDS_WIDTH + coords.x] = piece;
    }

    public void PlaceBlackPiece(Vector2Int coords)
    {
        Vector3 piecePosition = GetWorldPointCenteredAtCoords(coords);
        var piece = Instantiate(blackPiecePrefab);
        piece.transform.SetParent(pieceHolder);
        piece.transform.localPosition = piecePosition;

        pieces[coords.y * COORDS_WIDTH + coords.x] = piece;
    }

    public bool IsPointWithinBoardBounds(Vector2 point)
    {
        Rect geometryRect = new Rect(geometryStartPosition, geometrySize);

        return geometryRect.Contains(point);
    }

    public Vector2Int GetCoordsFromWorldPoint(Vector2 point)
    {
        Vector2 deltaPosition = point - geometryStartPosition;

        int coordsX = (int)(deltaPosition.x / TILE_SIZE);
        int coordsY = (int)(deltaPosition.y / TILE_SIZE);

        return new Vector2Int(coordsX, coordsY);
    }

    public Vector2 GetWorldPointCenteredAtCoords(Vector2Int coords)
    {
        Vector2 deltaPosition = new Vector2(coords.x * TILE_SIZE, coords.y * TILE_SIZE);

        return geometryStartPosition + Vector2.one * TILE_SIZE / 2f + deltaPosition;
    }
}
