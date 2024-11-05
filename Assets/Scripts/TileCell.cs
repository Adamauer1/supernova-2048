using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Tile tile;
    public Vector2Int coords;
    public bool Empty => tile == null;
}
