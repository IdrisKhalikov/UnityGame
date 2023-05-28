using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
//using Unity.VisualScripting;


namespace BSPTreeGeneration
{
    public class MapGenerator
    {
        private Tilemap tilemap;
        private Tile wall;
        private Tile floor;
        private MapVisualizer visualizer;

        public MapGenerator(Tilemap tilemap, Tile wall, Tile floor, MapVisualizer visualizer)
        {
            this.tilemap = tilemap;
            this.wall = wall;
            this.floor = null;
            this.visualizer = visualizer;
        }

        public void GenerateMap(int x, int y, int width, int height, int minWidth, int minHeight, int offset)
        {
            tilemap.ClearAllTiles();
            var rooms = BSPAlgorithm.GetRooms(new BoundsInt(x, y, 0, width, height, 0), minHeight, minWidth);
            var tiles = GenerateRoomsWithOffset(rooms, offset);
            tiles.UnionWith(GenerateCorrdiors(rooms));
            visualizer.Init(tilemap);
            visualizer.RenderMap(tiles);
        }

        private HashSet<Vector2Int> GenerateCorrdiors(List<BoundsInt> rooms)
        {
            var roomCenters = rooms.Select(room => Vector2Int.RoundToInt(room.center)).ToList();
            var corridors = new HashSet<Vector2Int>();
            var currentCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
            while(roomCenters.Count > 0)
            {
                var closest = roomCenters.OrderBy(point => Vector2.Distance(currentCenter, point)).First();
                //corridors.AddRange(BuildPath(currentCenter, closest));
                corridors.UnionWith(BuildPath(currentCenter, closest));
                roomCenters.Remove(currentCenter);
                currentCenter = closest;
            }

            return corridors;
        }

        private IEnumerable<Vector2Int> BuildPath(Vector2Int start, Vector2Int end)
        {
            while (start.y != end.y)
            {
                start += start.y < end.y ? Vector2Int.up : Vector2Int.down;
                yield return start + Vector2Int.left;
                yield return start + Vector2Int.right;
                yield return start;
            }

            var corner = start;

            while (start.x != end.x)
            {
                start += start.x < end.x ? Vector2Int.right : Vector2Int.left;
                yield return start + Vector2Int.up;
                yield return start + Vector2Int.down;
                yield return start;
            }

            if (corner != start)
                for (var i = -1; i < 2; i++)
                    for (var j = -1; j < 2; j++)
                        yield return corner + new Vector2Int(i, j);
        }

        private HashSet<Vector2Int> GenerateRoomsWithOffset(List<BoundsInt> rooms, int offset)
        {
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
            foreach (var room in rooms)
            {
                for (int x = offset; x < room.size.x - offset; x++)
                {
                    for (int y = offset; y < room.size.y - offset; y++)
                    {
                        Vector2Int position = (Vector2Int)room.min + new Vector2Int(x, y);
                        positions.Add(position);
                    }
                }
            }
            return positions;
        }

        private void DrawFloor(HashSet<Vector2Int> tiles)
        {
            foreach (var tile in tiles)
                tilemap.SetTile((Vector3Int)tile, wall);
        }
    }
}
