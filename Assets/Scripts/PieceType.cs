using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PieceResourceType {
    MENTAL,
    PHYSICAL,
    FINANCIAL,
    NULL
}

[System.Serializable]
public struct PieceType
{
    public Tile tile;
    public PieceResourceType resourceType;
}
