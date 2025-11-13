using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileCell Cell;
    public TileState State;
    public bool Locked = false;
    private TextMeshProUGUI m_text;
    private Image m_image; 

    private void Awake(){
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_image = GetComponent<Image>();
    }

    public void SetTileState(TileState state){
        this.State = state;

        // m_text.text = state.Element;
        m_image.sprite = state.ElementSprite;
    }

    public void PlaceTile(TileCell cell){
        if (this.Cell != null){
            this.Cell.Tile = null;
        }
        
        this.Cell = cell;

        this.Cell.Tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTile(TileCell cell){
      if (this.Cell.Tile != null){
            this.Cell.Tile = null;
        }
        
        this.Cell = cell;

        this.Cell.Tile = this;

        // transform.position = cell.transform.position;
        StartCoroutine(Animate(cell.transform.position, false));  
    }

    public void MergeTile(TileCell cell){
        if (this.Cell.Tile != null){
            this.Cell.Tile = null;
        }

        this.Cell = null;
        cell.Tile.Locked = true;


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
