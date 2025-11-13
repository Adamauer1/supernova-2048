using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Tile Tile;
    public Vector2Int Coords;
    public bool Empty => Tile == null;
}
