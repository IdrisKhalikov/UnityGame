using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New level asset", menuName ="2D/Tiles/Map Tile")]
public class MapTile : Tile
{
    public TileType Type;
}

public enum TileType
{
    Floor = 0,
    Wall = 1
}
