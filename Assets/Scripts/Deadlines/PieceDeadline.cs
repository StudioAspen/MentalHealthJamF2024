using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDeadline : MonoBehaviour {
    public bool activeDeadline = false;
    public void InitalizeDeadline() {
        activeDeadline = true;
    }
    public void LockIn(Piece piece) {
        if (activeDeadline) {
            FindObjectOfType<DeadlineManager>().LockInPiece(piece);
        }
    }
}
