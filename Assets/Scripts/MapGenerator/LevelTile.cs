using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile
{
    public Vector3Int Position;
    public TileType Type;

    public LevelTile(Vector3Int position)
    {
        Position = position;
        Type = TileType.Default;
    }

    public LevelTile(Vector3Int position, TileType type) : this(position)
    {
        Type = type;
    }

    //Сравниваем только по расположению,
    //чтобы разные тайлы могли друг друга перезаписывать.
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Position.GetHashCode();
            return hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is not LevelTile)
            return false;

        var other = obj as LevelTile;
        return Position == other.Position;
    }
}

public enum TileType
{
    Hole,
    Ground,
    Corridor,
    Default
}
