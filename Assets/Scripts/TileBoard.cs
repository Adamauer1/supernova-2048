using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TileBoard : MonoBehaviour
{
    private TileGrid m_grid;
    private List<Tile> m_tiles;
    [SerializeField] private TileState[] m_tileStates;
    [SerializeField] private Tile m_tilePrefab;

    private bool m_waiting = false;

    private void Awake() {
        m_grid = GetComponentInChildren<TileGrid>();
        m_tiles = new List<Tile>(16);
    }

    private void Start()
    {
        TouchInput.Instance.OnSwipeRight += TouchInput_OnSwipeRight;
        TouchInput.Instance.OnSwipeLeft += TouchInput_OnSwipeLeft;
        TouchInput.Instance.OnSwipeUp += TouchInput_OnSwipeUp;
        TouchInput.Instance.OnSwipeDown += TouchInput_OnSwipeDown;
        
        
        CreateTile();
        CreateTile();
    }

    private void OnDisable()
    {
        TouchInput.Instance.OnSwipeRight -= TouchInput_OnSwipeRight;
        TouchInput.Instance.OnSwipeLeft -= TouchInput_OnSwipeLeft;
        TouchInput.Instance.OnSwipeUp -= TouchInput_OnSwipeUp;
        TouchInput.Instance.OnSwipeDown -= TouchInput_OnSwipeDown;
    }

    private void TouchInput_OnSwipeRight(object sender, EventArgs eventArgs)
    {
        MoveTiles(Vector2Int.right, 2, -1, 0, 1);
    }

    private void TouchInput_OnSwipeLeft(object sender, EventArgs eventArgs)
    {
        MoveTiles(Vector2Int.left, 1, 1, 0, 1);
    }

    private void TouchInput_OnSwipeUp(object sender, EventArgs eventArgs)
    {
        MoveTiles(Vector2Int.up, 0, 1, 1, 1);
    }

    private void TouchInput_OnSwipeDown(object sender, EventArgs eventArgs)
    {
        MoveTiles(Vector2Int.down, 0, 1, 2, -1);
    }

    public void HandleInput(InputAction.CallbackContext context){
        float xDirection = context.ReadValue<Vector2>().x;
        float yDirection = context.ReadValue<Vector2>().y;

        if (xDirection == 1){
            //right
            MoveTiles(Vector2Int.right, 2, -1, 0, 1);
        }
        else if (xDirection == -1){
            //left
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if(yDirection == 1){
            //up
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (yDirection == -1){
            //down
            MoveTiles(Vector2Int.down, 0, 1, 2, -1);
        }
    }

    private void CreateTile(){
        //find empty grid cell and set it to that
        Tile tile = Instantiate(m_tilePrefab, m_grid.transform);
        tile.SetTileState(m_tileStates[0]);
        // tile.SetTileState(m_tileStates[6]);
        tile.PlaceTile(m_grid.FindEmptyCell());
        m_tiles.Add(tile);
    }

    private void MoveTiles(Vector2Int direction, int startX, int incX, int startY, int incY){
        if (m_waiting) return;
        bool changed = false;
        for (int x = startX; x >= 0 && x < 4; x += incX){
            for (int y = startY; y >= 0 && y < 4; y += incY) {
                TileCell cell = m_grid.GetCell(x,y);
                // check if cell has a tile
                if (!cell.Empty){
                    changed = MoveTile(direction, cell.Tile);
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
        TileCell adjacentCell = m_grid.GetAdjacentCell(direction, tile.Cell);

        while (adjacentCell != null){
            if (!adjacentCell.Empty){
                // check merge
                if (CheckMerge(tile, adjacentCell.Tile)){
                    MergeTile(tile, adjacentCell.Tile);
                    return true;
                }
                break;
            }

            newCell = adjacentCell;
            adjacentCell = m_grid.GetAdjacentCell(direction, adjacentCell);
        }

        if (newCell != null){
            tile.MoveTile(newCell);
            return true;
        }
        return false;
    }

    private bool CheckMerge(Tile mergedTile, Tile tile){
        if (mergedTile.State == tile.State && !tile.Locked){
            return true;
        }
        return false;
    }

    private void MergeTile(Tile mergedTile, Tile tile){
        m_tiles.Remove(mergedTile);
        // merge tiles
        mergedTile.MergeTile(tile.Cell);

        int currentStateIndex = -1;
        
        for (int i = 0; i < m_tileStates.Length; i++){
            if (tile.State == m_tileStates[i]){
                currentStateIndex = i;
            }
        }
        
        if (tile.State.Element == "Fe")
        {
            Debug.Log("SUPERNOVA!!!");
            tile.SetTileState(m_tileStates[currentStateIndex]);
            return;
        }

        int nextStateIndex = Mathf.Clamp(currentStateIndex + 1, 0, m_tileStates.Length - 1);

        tile.SetTileState(m_tileStates[nextStateIndex]);
    }


    private IEnumerator WaitForChanges()
        {
            m_waiting = true;

            yield return new WaitForSeconds(0.1f);

            m_waiting = false;

            foreach (Tile tile in m_tiles) {
                tile.Locked = false;
            }

            if (m_tiles.Count != 16) {
                CreateTile();
            }

            // if (CheckForGameOver()) {
            //     GameManager.Instance.GameOver();
            // }
    }
}
