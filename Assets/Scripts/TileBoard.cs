using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TileBoard : MonoBehaviour
{
    private TileGrid grid;
    private TileCell[] cells;
    [SerializeField] private GameObject tilePrefab;

    private void Awake(){
        grid = GetComponentInChildren<TileGrid>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start(){
        CreateTile();
        CreateTile();
    }

    public void HandleInput(InputAction.CallbackContext context){

    }

    public void CreateTile(){
        // find empty cell
        // create on that cell
        GameObject tile = Instantiate(tilePrefab, FindEmptyCell().transform);
    }

    private TileCell FindEmptyCell(){
        int newIndex = Random.Range(1, cells.Length);
        
        return cells[newIndex];
        // while (cells[newIndex] != null){

        // }

        // return null;
    }


}
