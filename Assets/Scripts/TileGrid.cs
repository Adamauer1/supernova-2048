using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private TileRow[] rows;
    private TileCell[] cells;

    private void Awake(){
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();

        for (int i = 0; i < cells.Length; i++){
            cells[i].coords = new Vector2Int (i % 4, i / 4);
        }
    }

    public TileCell GetCell(Vector2Int coords){
        return GetCell(coords.x, coords.y);
    }

    public TileCell GetCell(int x, int y){
        if (x >= 0 && x < 4 && y >= 0 && y < 4){
            return rows[y].cells[x];
        }
        else {
            return null;
        }
    }

    public TileCell GetAdjacentCell(Vector2Int direction, TileCell cell){
        Vector2Int coords = cell.coords;

        coords.x += direction.x;
        coords.y -= direction.y;

        return GetCell(coords);
    }


    public TileCell FindEmptyCell(){
        System.Random random = new System.Random();
        TileCell[] randomCells = cells.OrderBy(x => random.Next()).ToArray();

        foreach (TileCell cell in randomCells){
            if (cell.Empty){
                return cell;
            }
        }
        return null;
        // int newCellIndex = Random.Range(0, cells.Length);

        // int loopIndexCheck = newCellIndex;

        // while(!cells[newCellIndex].Empty){
        //     newCellIndex = (newCellIndex + 1) % cells.Length;

        //     if (loopIndexCheck == newCellIndex){
        //         // no empty spots
        //         return null;
        //     }
        // }
        
        //return cells[newCellIndex];
    }
}
