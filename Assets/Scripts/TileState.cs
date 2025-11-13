using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    //change to image
    public string Element;
    public Color BackgroundColor;
    public Color TextColor;
    public Sprite ElementSprite;
}
