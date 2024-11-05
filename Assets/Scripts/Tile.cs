using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public TileCell cell;
    public TileState state;
    public bool locked = false;
    private TextMeshProUGUI text;

    private void Awake(){
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTileState(TileState state){
        this.state = state;

        text.text = state.element;
    }

    public void PlaceTile(TileCell cell){
        if (this.cell != null){
            this.cell.tile = null;
        }
        
        this.cell = cell;

        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTile(TileCell cell){
      if (this.cell.tile != null){
            this.cell.tile = null;
        }
        
        this.cell = cell;

        this.cell.tile = this;

        // transform.position = cell.transform.position;
        StartCoroutine(Animate(cell.transform.position, false));  
    }

    public void MergeTile(TileCell cell){
        if (this.cell.tile != null){
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;


        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool merging){
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) {
            Destroy(gameObject);
        }
    }
}
