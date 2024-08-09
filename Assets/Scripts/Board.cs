using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    [SerializeField] Slider burnSlider;
    [SerializeField] GameObject gamePiecePrefab;
    [SerializeField] int initalPieceAmount;
    [SerializeField] float burnRate;
    float burnTimer;
    [SerializeField] float spawnRate;
    float spawnTimer = 0;

    public Tilemap tilemap { get; private set; }
    public List<Piece> activePieces = new List<Piece>();

    public PieceType[] pieceTypes;
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(0, 8, 0);

    bool gameActive = false;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        // Initalizing some pieces
        for(int i = 0; i < initalPieceAmount; i++) {
            SpawnPiece();
        }
    }
    private void Update() {
        if (gameActive) {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0) {
                SpawnPiece();
                spawnTimer = spawnRate;
            }

            burnTimer -= Time.deltaTime;
            if(burnTimer <= 0) {
                BurnBottom();
                burnTimer = burnRate;
            }

            burnSlider.value = 1f - burnTimer / burnRate;
        }
    }

    public void SetGameActive(bool val) {
        gameActive = val;
    }

    public void SpawnPiece()
    {
        int randomTetromino = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[randomTetromino];

        int randomPieceType = Random.Range(0, pieceTypes.Length);
        PieceType type = pieceTypes[randomPieceType];

        Piece newPiece = Instantiate(gamePiecePrefab).GetComponent<Piece>();
        newPiece.Initialize(this, spawnPosition, data, type);

        if (TryGetRandomSpawnPos(newPiece, out Vector3Int randomPos)) {
            newPiece.SetPosition(randomPos);
            activePieces.Add(newPiece);
            Set(newPiece);
        } 
        else {
            Destroy(newPiece.gameObject);
            GameOver();
        }
    }

    private void BurnBottom() {
        foreach (Piece piece in activePieces) {
            if (piece.locked) {
                piece.LockedMoveDown();
            }
        }
    }

    private bool TryGetRandomSpawnPos(Piece newPiece, out Vector3Int randomPos) {

        List<Vector3Int> validPlaces = new List<Vector3Int>();
        Vector3Int topLeft = new Vector3Int(-boardSize.x/2, boardSize.y/2);
        //Debug.Log(topLeft);
        for(int i = 0; i < boardSize.y; i++) {

            // Finding all valid places on row
            validPlaces.Clear();
            for(int j = 0; j < boardSize.x; j++) {
                Vector3Int checkPos = topLeft + new Vector3Int(j,-i,0);
                if(IsValidPosition(newPiece, checkPos)) {
                    validPlaces.Add(checkPos);
                } 
            }

            if(validPlaces.Count > 0) {
                int randomPlace = Random.Range(0,validPlaces.Count);
                randomPos = validPlaces[randomPlace];
                return true;
            }
        }
        

        randomPos = new Vector3Int();
        return false;
    }

    public void GameOver()
    {
        FindObjectOfType<GameManager>().LoseGame("Oh no! You got overwhelmed.");
        tilemap.ClearAllTiles();
        // Do anything else you want on game over here..
    }

    public Piece GetPieceAtCell(Vector3Int cellPos) {
        Piece output = null;

        foreach(Piece piece in activePieces) {
            foreach(Vector3Int pieceCell in piece.cells) {
                if(cellPos == pieceCell+piece.position) {
                    return piece;
                }
            }
        }

        return output;
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;

            // Only drawing tile if its in bounds
            RectInt bounds = Bounds;
            if (bounds.Contains((Vector2Int)tilePosition)) {
                tilemap.SetTile(tilePosition, piece.pieceType.tile);
                if (piece.locked) {
                    tilemap.SetTileFlags(tilePosition, TileFlags.None);
                    tilemap.SetColor(tilePosition, Color.grey);
                }
            }
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
            tilemap.SetColor(tilePosition, Color.white);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

}
