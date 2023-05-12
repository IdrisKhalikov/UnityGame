using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapVisualizer : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField]
    public TileBase Floor, LB, LU, RB, RU, LM, BM, RM, UM, CLU, CRU, CRB, CLB;
    public Dictionary<int, TileBase> tileMasks;

    public void Init(Tilemap tilemap)
    {
        this.tilemap = tilemap;
        tileMasks = new Dictionary<int, TileBase>
        {
            {0b11111111, Floor },
            {0b01101000, LU },
            {0b11101000, LU },
            {0b01101001, LU },
            {0b11101001, LU },
            {0b11111000, UM },
            {0b11111100, UM },
            {0b11111001, UM },
            {0b11111101, UM },
            {0b11010000, RU },
            {0b11110000, RU },
            {0b11010100, RU },
            {0b11110100, RU },
            {0b11010110, RM },
            {0b11110110, RM },
            {0b11010111, RM },
            {0b11110111, RM },
            {0b00010110, RB },
            {0b10010110, RB },
            {0b00010111, RB },
            {0b10010111, RB },
            {0b00011111, BM },
            {0b10011111, BM },
            {0b00111111, BM },
            {0b10111111, BM },
            {0b00001011, LB },
            {0b00101011, LB },
            {0b00001111, LB },
            {0b00101111, LB },
            {0b01101011, LM },
            {0b11101011, LM },
            {0b01101111, LM },
            {0b11101111, LM },
            {0b11111011, CLU },
            {0b11111110, CRU },
            {0b11011111, CRB },
            {0b01111111, CLB }
        };
    }

    public void RenderMap(HashSet<Vector2Int> tiles)
    {
        foreach(var tile in tiles)
            tilemap.SetTile((Vector3Int)tile, GetTileBase(tile, tiles));
    }

    public TileBase GetTileBase(Vector2Int tile, HashSet<Vector2Int> tiles)
    {
        var mask = Enumerable.Range(-1, 3)
            .SelectMany(y => Enumerable.Range(-1, 3)
                .Select(x => new Vector2Int(x, y)))
            .Where(p => p != Vector2Int.zero)
            .Aggregate(0, (current, next) => (current << 1) | (tiles.Contains(next + tile) ? 1 : 0));
        if(tileMasks.ContainsKey(mask))
        {
            return tileMasks[mask];
        }
        else
        {
            Debug.Log($"Replaced {mask} with floor");
            return Floor;
        }
    }
}
