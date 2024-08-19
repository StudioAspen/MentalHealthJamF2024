using UnityEngine;

public class Piece : MonoBehaviour {
    [SerializeField] AudioSource lockSound;
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public PieceType pieceType { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;
    public float resourceGain = 2f;

    private float stepTime;
    private float moveTime;
    private float lockTime;
    public bool locked { get; private set; } 
    public bool deadline { get; private set; }
    public Color deadlineColor;
    public bool canRender = true;

    public void Initialize(Board board, Vector3Int position, TetrominoData data, PieceType pieceType)
    {
        this.data = data;
        this.pieceType = pieceType;
        this.board = board;
        this.position = position;

        rotationIndex = 0;
        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (cells == null) {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        board.Clear(this);

        // We use a timer to allow the player to make adjustments to the piece
        // before it locks in place
        lockTime += Time.deltaTime;

        // Handle hard drop
        if (Input.GetKeyDown(KeyCode.Space)) {
            //HardDrop();
        }

        // Allow the player to hold movement keys but only after a move delay
        // so it does not move too fast
        if (Time.time > moveTime) {
            //HandleMoveInputs();
        }

        // Advance the piece to the next row every x seconds
        if (Time.time > stepTime) {
            //Step();
        }

        if (canRender) {
            board.Set(this);
        }

        DestroyCheck();
    }

    public void InitalizeDeadline(DeadlineData data) {
        deadline = true;
        deadlineColor = data.color;
    }

    private void DestroyCheck() {
        int highestY = -board.boardSize.y;
        foreach (Vector3Int cell in cells) {
            int heightCheck = (cell + position).y;
            if (heightCheck > highestY) {
                highestY = heightCheck;
            }
        }

        if (highestY < -board.boardSize.y / 2) {
            DestroyPiece();
        }
    }

    public GameObject ClonePiece() {
        GameObject newPiece = Instantiate(gameObject);
        newPiece.GetComponent<Piece>().Initialize(board, position, data, pieceType);
        newPiece.GetComponent<Piece>().SetRotation(rotationIndex);
        return newPiece;
    }

    public void SetBlock(bool val) {
        locked = val;
        if (!canRender) return;
        if(locked) {
            if(!lockSound.isPlaying) {
                lockSound.Play();
            }

            if (deadline) {
                FindObjectOfType<DeadlineManager>().LockInPiece(this);
            }
            else {
                ResourceManager resourceManager = FindObjectOfType<ResourceManager>();
                switch (pieceType.resourceType) {
                    case PieceResourceType.PHYSICAL:
                        Debug.Log($"Physical +{resourceGain}");
                        resourceManager.AddPhysical(resourceGain);
                        break;
                    case PieceResourceType.MENTAL:
                        Debug.Log($"Mental +{resourceGain}");
                        resourceManager.AddMental(resourceGain);
                        break;
                    case PieceResourceType.FINANCIAL:
                        Debug.Log($"Financial +{resourceGain}");
                        resourceManager.AddFinancial(resourceGain);
                        break;
                }
            }
        }
    }

    public void CanRender(bool val) {
        canRender = val;
    }

    public void SetPosition(Vector3Int pos) {
        Vector3Int difference = pos-position;
        Move((Vector2Int)difference);
    }

    private void HandleMoveInputs()
    {
        // Soft drop movement
        if (Input.GetKey(KeyCode.S))
        {
            if (Move(Vector2Int.down)) {
                // Update the step time to prevent double movement
                stepTime = Time.time + stepDelay;
            }
        }

        // Left/right movement
        if (Input.GetKey(KeyCode.A)) {
            Move(Vector2Int.left);
        } else if (Input.GetKey(KeyCode.D)) {
            Move(Vector2Int.right);
        }
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        // Step down to the next row
        Move(Vector2Int.down);

        // Once the piece has been inactive for too long it becomes locked
        if (lockTime >= lockDelay) {
            Lock();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock();
    }

    private void Lock()
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }

    public void LockedMoveDown() {
        // Setting new position
        Vector3Int newPosition = position;
        newPosition.y--;

        board.Clear(this);
        position = newPosition;
        board.Set(this);

    }

    public void DestroyPiece() {
        board.Clear(this);
        board.activePieces.Remove(this);
        Destroy(gameObject);
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid) {
            board.Clear(this);
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f; // reset
            if (canRender) {
                if (!locked)
                {
                    CollisionCheck();
                }
            }
            board.Set(this);
        }

        return valid;
    }

    private void CollisionCheck() {
        foreach(Vector3Int cell in cells) {
            Vector3Int posCheck = cell + position;
            Piece potentialPiece = board.GetPieceAtCell(posCheck + Vector3Int.down);
            if (locked) return;
            if (posCheck.y <= -board.boardSize.y / 2)
            {
                SetBlock(true);
            }
            else if (potentialPiece) {
                if(potentialPiece.locked) {
                    SetBlock(true);
                }
            }
        }
    }

    public void Rotate(int direction)
    {
        // Store the current rotation in case the rotation fails
        // and we need to revert
        int originalRotation = rotationIndex;

        // Rotate all of the cells using a rotation matrix
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        // Revert the rotation if the wall kick tests fail
        if (!TestWallKicks(rotationIndex, direction))
        {
            Debug.Log("Wall kick");
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }
    public void SetRotation(int newRotationIndex) {
        int limit = 10;
        board.Clear(this);
        int direction = 1;
        while(rotationIndex != newRotationIndex && limit > 0) {
            rotationIndex = Wrap(rotationIndex + direction, 0, 4);
            ApplyRotationMatrix(direction);
            //Debug.Log(rotationIndex + " " + newRotationIndex);
            limit--;
        }
        board.Set(this);
    }
    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation)) {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }

}
