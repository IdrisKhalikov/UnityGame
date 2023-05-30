using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New level asset", menuName ="2D/Tiles/Map Tile")]
public class MapTile : Tile
{
    public TileType Type;
}

