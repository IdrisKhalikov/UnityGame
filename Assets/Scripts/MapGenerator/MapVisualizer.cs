using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEditor;

public class MapVisualizer
{
    private Tilemap tilemap;
    private TileType tileType;
    private Dictionary<int, TileBase> tileMasks;

    public MapVisualizer(TileType tileType, Tilemap tilemap)
    {
        this.tileType = tileType;
        this.tilemap = tilemap;
        Init();
    }

    public void Init()
    {
        tileMasks = new Dictionary<int, TileBase>
        {
            {0b11111111, LoadTile("Floor") },
            {0b01101000, LoadTile("LU") },
            {0b11101000, LoadTile("LU") },
            {0b01101001, LoadTile("LU") },
            {0b11101001, LoadTile("LU") },
            {0b11111000, LoadTile("UM") },
            {0b11111100, LoadTile("UM") },
            {0b11111001, LoadTile("UM") },
            {0b11111101, LoadTile("UM") },
            {0b11010000, LoadTile("RU") },
            {0b11110000, LoadTile("RU") },
            {0b11010100, LoadTile("RU") },
            {0b11110100, LoadTile("RU") },
            {0b11010110, LoadTile("RM") },
            {0b11110110, LoadTile("RM") },
            {0b11010111, LoadTile("RM") },
            {0b11110111, LoadTile("RM") },
            {0b00010110, LoadTile("RB") },
            {0b10010110, LoadTile("RB") },
            {0b00010111, LoadTile("RB") },
            {0b10010111, LoadTile("RB") },
            {0b00011111, LoadTile("BM") },
            {0b10011111, LoadTile("BM") },
            {0b00111111, LoadTile("BM") },
            {0b10111111, LoadTile("BM") },
            {0b00001011, LoadTile("LB") },
            {0b00101011, LoadTile("LB") },
            {0b00001111, LoadTile("LB") },
            {0b00101111, LoadTile("LB") },
            {0b01101011, LoadTile("LM") },
            {0b11101011, LoadTile("LM") },
            {0b01101111, LoadTile("LM") },
            {0b11101111, LoadTile("LM") },
            {0b11111011, LoadTile("CLU") },
            {0b11111110, LoadTile("CRU") },
            {0b11011111, LoadTile("CRB") },
            {0b01111111, LoadTile("CLB") }
        };
    }

    private TileBase LoadTile(string name)
    {
        return Resources.Load<TileBase>($"MapTiles/{tileType}/{name}");
    }

    public void RenderMap(HashSet<LevelTile> tiles, HashSet<LevelTile> allTiles)
    {
        foreach (var tile in tiles)
        {
            if (tile.Type == TileType.Corridor || tile.Type == TileType.Ground)
                tilemap.SetTile((Vector3Int)tile.Position, GetTileBase(tile, allTiles));
        }
    }

    public TileBase GetTileBase(LevelTile tile, HashSet<LevelTile> tiles)
    {
        var mask = Enumerable.Range(-1, 3)
            .SelectMany(y => Enumerable.Range(-1, 3)
                .Select(x => new Vector3Int(x, y)))
            .Where(p => p != Vector3Int.zero)
            .Aggregate(0, (current, next)
            => (current << 1) | (tiles.Contains(new LevelTile(tile.Position + next)) ? 1 : 0));
        if (tileMasks.ContainsKey(mask))
        {
            return tileMasks[mask];
        }
        else
        {
            Debug.Log($"Replaced {mask} with floor");
            return tileMasks[0];
        }
    }
}

class SimpleVisualizer
{
    private Tilemap tilemap;
    private TileType tileType;
    private TileBase tileBase;

    public SimpleVisualizer(Tilemap tilemap, TileType tileType)
    {
        this.tilemap = tilemap;
        this.tileType = tileType;
        tileBase = Resources.Load<TileBase>($"MapTiles/{tileType}/Floor");
    }

    public void RenderMap(HashSet<LevelTile> tiles)
    {
        foreach (var tile in tiles)
        {
            tilemap.SetTile(tile.Position, tileBase);
        }
    }
}

class RandomVisualizer
{
    private Tilemap tilemap;
    private List<TileBase> tileBases;

    public RandomVisualizer(Tilemap tilemap, List<TileBase> tiles)
    {
        this.tilemap = tilemap;
        tileBases = tiles;
        Debug.Log(tiles.Count);
    }

    public void RenderMap(List<BoundsInt> rooms, int minCount, int maxCount)
    {
        var placedPositions = new HashSet<Vector3Int>();
        foreach (var room in rooms)
        {
            var amount = Random.Range(minCount, maxCount);
            for (var i = 0; i < amount; i++)
            {
                var x = Random.Range(2, room.size.x - 2);
                var y = Random.Range(2, room.size.y - 2);
                var position = new Vector3Int(x, y) + room.min;
                if (placedPositions.Contains(position))
                    continue;
                tilemap.SetTile(position, tileBases[Random.Range(0, tileBases.Count)]);
                placedPositions.Add(position);
            }
        }
    }
}
