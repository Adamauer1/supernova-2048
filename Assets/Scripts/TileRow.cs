using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] Cells;

    private void Awake(){
        Cells = GetComponentsInChildren<TileCell>();
    }
}
