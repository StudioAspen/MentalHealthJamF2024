using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TetrisTemplate {
    public class PlayerInteract : MonoBehaviour {
        Board board;
        Ghost ghost;
        [SerializeField] Piece ghostCopy;
        Piece currentActivePiece;

        private void Start() {
            board = FindObjectOfType<Board>();
            ghost = FindObjectOfType<Ghost>();
            DisableGhost();
        }

        private void Update() {
            InputHander();

            // Moving ghost
            if(currentActivePiece) {
                // Handle rotation
                if (Input.GetKeyDown(KeyCode.Q)) {
                    ghostCopy.Rotate(-1);
                }
                else if (Input.GetKeyDown(KeyCode.E)) {
                    ghostCopy.Rotate(1);
                }

                // Setting position
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                ghostCopy.SetPosition(board.tilemap.WorldToCell(worldPos));
            }
        }

        private void InputHander() {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 worldPosCheck = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Debug.Log(board.tilemap.WorldToCell(worldPosCheck));
                if (currentActivePiece) { // Setting position of current active
                    currentActivePiece.SetRotation(ghostCopy.rotationIndex);
                    currentActivePiece.SetPosition(ghostCopy.position);

                    // Resetting values
                    currentActivePiece = null;
                    DisableGhost();
                }
                else { // Getting at mouse pos current active
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));    

                    currentActivePiece = board.GetPieceAtCell(board.tilemap.WorldToCell(worldPos)); // Getting active piece at cell
                    if(currentActivePiece != null) {
                        if (!currentActivePiece.locked) {
                            SetGhost(currentActivePiece.ClonePiece().GetComponent<Piece>());
                        }
                        else {
                            currentActivePiece = null;
                        }
                    }
                }
            }
        }

        private void SetGhost(Piece piece) {
            ghostCopy = piece;
            ghostCopy.CanRender(false);
            ghost.trackingPiece = ghostCopy;
            ghost.gameObject.SetActive(true);
        }

        private void DisableGhost() {
            if (ghostCopy) {
                Destroy(ghostCopy.gameObject);
            }
            ghost.gameObject.SetActive(false);
        }
    }
}