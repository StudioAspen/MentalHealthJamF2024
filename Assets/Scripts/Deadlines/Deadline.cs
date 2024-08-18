using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadline : MonoBehaviour {
    DeadlineData data;
    List<Piece> pieces;
    public float maxDuration;
    public float timer;

    private void Update() {
        timer -= Time.deltaTime;
    }

    public void InitalizeDeadline(DeadlineData data, List<Piece> pieces, float maxDuration) {
        this.data = data;
        this.pieces = pieces;
        this.maxDuration = maxDuration;
        timer = maxDuration;
    }

    public void EndDeadline() {
        foreach (Piece piece in pieces) {
            piece.DestroyPiece();
        }
        Destroy(gameObject);
    }

    public void TryLockInPiece(Piece piece) {
        if(pieces.Contains(piece)) {
            pieces.Remove(piece);
        }
    }
}
