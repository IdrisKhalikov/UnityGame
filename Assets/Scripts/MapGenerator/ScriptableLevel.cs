using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableLevel : ScriptableObject
{
    public int LevelId;
    public List<SavedTile> Tiles;
}

[Serializable]
public class SavedTile
{
    public Vector3Int Position;
    public MapTile Tile;
}
