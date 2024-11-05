using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TileBoard : MonoBehaviour
{
    private TileGrid grid;
    private List<Tile> tiles;
    [SerializeField] private TileState[] tileStates;
    [SerializeField] private Tile tilePrefab;

    private bool waiting = false;

    private void Awake() {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    private void Start(){
        CreateTile();
        CreateTile();
    }

    public void HandleInput(InputAction.CallbackContext context){
        float XDirection = context.ReadValue<Vector2>().x;
        float YDirection = context.ReadValue<Vector2>().y;

        if (XDirection == 1){
            //right
            MoveTiles(Vector2Int.right, 2, -1, 0, 1);
        }
        else if (XDirection == -1){
            //left
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if(YDirection == 1){
            //up
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (YDirection == -1){
            //down
            MoveTiles(Vector2Int.down, 0, 1, 2, -1);
        }
    }

    private void CreateTile(){
        //find empty grid cell and set it to that
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetTileState(tileStates[0]);
        tile.PlaceTile(grid.FindEmptyCell());
        tiles.Add(tile);
    }

    private void MoveTiles(Vector2Int direction, int startX, int incX, int startY, int incY){
        bool changed = false;
        for (int x = startX; x >= 0 && x < 4; x += incX){
            for (int y = startY; y >= 0 && y < 4; y += incY) {
                TileCell cell = grid.GetCell(x,y);
                // check if cell has a tile
                if (!cell.Empty){
                    changed = MoveTile(direction, cell.tile);
                }
                // if it does then move tile
            }
        }

        if (changed){
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Vector2Int direction, Tile tile){
        TileCell newCell = null;
        TileCell adjacentCell = grid.GetAdjacentCell(direction, tile.cell);

        while (adjacentCell != null){
            if (!adjacentCell.Empty){
                // check merge
                if (CheckMerge(tile, adjacentCell.tile)){
                    MergeTile(tile, adjacentCell.tile);
                    return true;
                }
                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(direction, adjacentCell);
        }

        if (newCell != null){
            tile.MoveTile(newCell);
            return true;
        }
        return false;
    }

    private bool CheckMerge(Tile mergedTile, Tile tile){
        if (mergedTile.state == tile.state && !tile.locked && tile.state.element != "Fe"){
            return true;
        }
        return false;
    }

    private void MergeTile(Tile mergedTile, Tile tile){
        tiles.Remove(mergedTile);
        // merge tiles
        mergedTile.MergeTile(tile.cell);


        int currentStateIndex = -1;
        for (int i = 0; i < tileStates.Length; i++){
            if (tile.state == tileStates[i]){
                currentStateIndex = i;
            }
        }

        int nextStateIndex = Mathf.Clamp(currentStateIndex + 1, 0, tileStates.Length - 1);

        tile.SetTileState(tileStates[nextStateIndex]);
    }


    private IEnumerator WaitForChanges()
        {
            waiting = true;

            yield return new WaitForSeconds(0.1f);

            waiting = false;

            foreach (var tile in tiles) {
                tile.locked = false;
            }

            if (tiles.Count != 16) {
                CreateTile();
            }

            // if (CheckForGameOver()) {
            //     GameManager.Instance.GameOver();
            // }
    }
}
