using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    //change to image
    public string element;
    public Color backgroundColor;
    public Color textColor;
}
