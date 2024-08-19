using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineManager : MonoBehaviour
{
    [SerializeField] int spawnLimit;
    [SerializeField] GameObject deadlineLayout;
    [SerializeField] GameObject deadlinePrefab;
    [SerializeField] Vector2 deadlineSpawnRate;
    [SerializeField] Vector2 deadlineSpawnAmount;
    [SerializeField] Vector2 deadlineDuration;
    [SerializeField] List<DeadlineData> deadlineDatas;
    [SerializeField] AudioSource deadlineCreation;
    public float spawnTimer;
    bool gameActive;

    private void Start() {
        spawnTimer = Random.Range(deadlineSpawnAmount.x, deadlineSpawnAmount.y);
    }
    private void Update() {
        if(gameActive) {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0) {
                SpawnDeadline();
                spawnTimer = Random.Range(deadlineSpawnRate.x, deadlineSpawnRate.y);
            }
        }
    }

    public void StartGame() {
        gameActive = true;
    }
    public void EndGame() {
        gameActive = false;
    }

    public void SpawnDeadline() {
        if (GetComponentsInChildren<Deadline>().Length < spawnLimit) {
            if(!deadlineCreation.isPlaying) {
                deadlineCreation.Play();
            }

            int spawnAmount = Mathf.RoundToInt(Random.Range(deadlineSpawnAmount.x, deadlineSpawnAmount.y));
            Board board = FindObjectOfType<Board>();
            int randomDeadlineIndex = Random.Range(0, deadlineDatas.Count);

            // Initalizing deadline values
            List<Piece> deadlinePieces = new List<Piece>();
            for (int i = 0; i < spawnAmount; i++) {
                Piece newPiece = board.SpawnPiece();
                newPiece.InitalizeDeadline(deadlineDatas[randomDeadlineIndex]);
                deadlinePieces.Add(newPiece);
            }
            GameObject newDeadline = Instantiate(deadlinePrefab, deadlineLayout.transform);

            // Getting Random Range
            newDeadline.GetComponent<Deadline>().InitalizeDeadline(deadlineDatas[randomDeadlineIndex], deadlinePieces, Random.Range(deadlineSpawnRate.x, deadlineSpawnRate.y));
        }
    }

    public void LockInPiece(Piece piece) {
        Deadline[] deadlines = GetComponentsInChildren<Deadline>();
        foreach(Deadline deadline in deadlines) {
            deadline.TryLockInPiece(piece);
        }
    }
}
