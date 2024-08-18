using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineManager : MonoBehaviour
{
    [SerializeField] GameObject deadlineLayout;
    [SerializeField] GameObject deadlinePrefab;
    [SerializeField] Vector2 deadlineSpawnRate;
    [SerializeField] List<DeadlineData> deadlineDatas;
    public void SpawnDeadline() {
        int spawnAmount = Mathf.RoundToInt(Random.Range(deadlineSpawnRate.x, deadlineSpawnRate.y));
        Board board = FindObjectOfType<Board>();

        // Initalizing deadline values
        List<Piece> deadlinePieces = new List<Piece>();
        for(int i = 0; i < spawnAmount; i++) {
            Piece newPiece = board.SpawnPiece();
            newPiece.GetComponent<PieceDeadline>().InitalizeDeadline();
            deadlinePieces.Add(newPiece);
        }
        GameObject newDeadline = Instantiate(deadlinePrefab, deadlineLayout.transform);

        // Getting Random Range
        int randomIndex = Random.Range(0, deadlineDatas.Count);
        newDeadline.GetComponent<Deadline>().InitalizeDeadline(deadlineDatas[randomIndex], deadlinePieces, Random.Range(deadlineSpawnRate.x, deadlineSpawnRate.y));

    }

    public void LockInPiece(Piece piece) {
        Deadline[] deadlines = GetComponentsInChildren<Deadline>();
        foreach(Deadline deadline in deadlines) {
            deadline.TryLockInPiece(piece);
        }
    }
}
