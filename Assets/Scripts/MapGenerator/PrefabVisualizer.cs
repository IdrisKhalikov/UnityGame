using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrefabVisualizer
{
    private List<GameObject> prefabs;
    private Tilemap tilemap;

    public PrefabVisualizer(List<GameObject> prefabs, Tilemap tilemap)
    {
        this.prefabs = prefabs;
        this.tilemap = tilemap;
    }

    public void RenderMap(List<BoundsInt> rooms, int amount)
    {
        var placedPositions = new HashSet<Vector3>();
        while(amount > 0)
        {
            foreach (var room in rooms)
            {
                if (Random.Range(0, 2) == 0)
                    continue;
                if (amount == 0)
                    break;
                amount--;
                var x = Random.Range(2, room.size.x - 1);
                var y = Random.Range(2, room.size.y - 1);
                var position = tilemap.CellToWorld(new Vector3Int(x, y) + room.min);
                if (placedPositions.Contains(position))
                    continue;
                GameObject.Instantiate(prefabs[Random.Range(0, prefabs.Count)], position, Quaternion.Euler(0, 0, 0));
                placedPositions.Add(position);
            }
        }

    }
}
